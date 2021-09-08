using System;
using System.Text.Json;
using System.Threading;
using CommentService.DAL;
using CommentService.Integration.Dto;
using CommentService.Integration.Repositories;
using Confluent.Kafka;
using Outbox.Consumer.Events;
using Outbox.Consumer.Repositories;

namespace CommentService.Integration
{
    public class PostsConsumer
    {
        private readonly string _topic;
        private readonly IConsumer<string, string> _kafkaConsumer;
        
        private readonly ConsumedMessageRepository<CommentDbContext, Post> _consumedMessageRepository;
        private readonly PostRepository _postRepository;

        public PostsConsumer(ConsumedMessageRepository<CommentDbContext, Post> consumedMessageRepository, PostRepository postRepository)
        {
            _consumedMessageRepository = consumedMessageRepository;
            _postRepository = postRepository;
            
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "blog-service-kafka-cp-kafka-headless:9092",
                GroupId = "CommentService"
            };
            _topic = "Post.events";
            _kafkaConsumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        public void StartConsumerLoop(CancellationToken cancellationToken)
        {
            Console.WriteLine($"CommentsService: subscribing to topic: {_topic}");
            _kafkaConsumer.Subscribe(_topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var receivedEvent = ProcessOutboxEvent(_kafkaConsumer.Consume(cancellationToken));
                    if (!_consumedMessageRepository.HasBeenConsumed(receivedEvent))
                    {
                        _consumedMessageRepository.Add(receivedEvent);
                         // Do some folder stuff
                         OnReceiveEvent(receivedEvent);
                    }
                    else
                    {
                        Console.WriteLine(
                            $"Comments service received message {receivedEvent.Id} which has already been consumed");                        
                    }

                    
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    // Consumer errors should generally be ignored (or logged) unless fatal.
                    Console.WriteLine($"Consume error: {e.Error.Reason}");

                    if (e.Error.IsFatal)
                    {
                        // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e}");
                    break;
                }
            }
        }

        public void Dispose()
        {
            _kafkaConsumer.Close();
            _kafkaConsumer.Dispose();
        }

        private void OnReceiveEvent(ReceivedEvent<Post> receivedEvent)
        {
            Console.WriteLine($"CommentService.Integration.PostsBackgroundService has the event {receivedEvent.Id} {receivedEvent.Type}");
            if (receivedEvent.Type == "PostCreated")
            {
                _postRepository.Add(receivedEvent.Payload);
            }
            else
            {
                Console.WriteLine($"CommentService.Integration.PostsBackgroundService HAS RECEIVED AN UNKNOWN EVENT {receivedEvent.Id} {receivedEvent.Type}");
            }
        }
        
        private ReceivedEvent<Post> ProcessOutboxEvent(ConsumeResult<string, string> consumeResult)
        {
            var messageIdBuffer = consumeResult.Message.Headers.GetLastBytes("id");
            var id = System.Text.Encoding.UTF8.GetString(messageIdBuffer, 0, messageIdBuffer.Length);
            
            var eventTypeBuffer = consumeResult.Message.Headers.GetLastBytes("eventType");
            var type = System.Text.Encoding.UTF8.GetString(eventTypeBuffer, 0, eventTypeBuffer.Length);

            return new ReceivedEvent<Post>(id, type, JsonSerializer.Deserialize<Post>(consumeResult.Message.Value));
        }
    }
}

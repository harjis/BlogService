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
        
        private readonly ConsumedEventRepository<CommentDbContext, Post> _consumedEventRepository;
        private readonly PostRepository _postRepository;

        public PostsConsumer(ConsumedEventRepository<CommentDbContext, Post> consumedEventRepository, PostRepository postRepository)
        {
            _consumedEventRepository = consumedEventRepository;
            _postRepository = postRepository;
            
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "primary-kafka.primary-kafka.svc.cluster.local:9071",
                GroupId = "CommentService",
                // The offset to start reading from if there are no committed offsets (or there was an error in retrieving offsets).
                AutoOffsetReset = AutoOffsetReset.Earliest,
                // Do not commit offsets.
                EnableAutoCommit = false
            };
            _topic = "outbox";
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
                    if (!_consumedEventRepository.HasBeenConsumed(receivedEvent))
                    {
                        _consumedEventRepository.Add(receivedEvent);
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
                    Console.WriteLine($"Consume error: {e.Error.Reason} was fatal: {e.Error.IsFatal}");

                    if (e.Error.IsFatal)
                    {
                        Console.WriteLine("Fatal error: killing the background process");
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
            switch (receivedEvent.Type)
            {
                case "PostCreated":
                    _postRepository.Add(receivedEvent.Payload);
                    break;
                case "PostUpdated":
                    _postRepository.Update(receivedEvent.Payload);
                    break;
                case "PostDeleted":
                    _postRepository.Delete(receivedEvent.Payload.Id);
                    break;
                default:
                    Console.WriteLine($"CommentService.Integration.PostsBackgroundService HAS RECEIVED AN UNKNOWN EVENT {receivedEvent.Id} {receivedEvent.Type}");
                    break;
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

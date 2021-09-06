using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Outbox.Consumer.Events;
using Outbox.Consumer.Models;
using Outbox.Consumer.Repositories;

namespace Outbox.Consumer
{
    public class Builder<TContext, TPayload> where TContext : DbContext
    {
        private readonly string _topic;
        private readonly IConsumer<string, string> _kafkaConsumer;

        private readonly ConsumedMessageRepository<TContext, TPayload> _consumedMessageRepository;

        public Builder(ConsumedMessageRepository<TContext, TPayload> consumedMessageRepository)
        {
            _consumedMessageRepository = consumedMessageRepository;
            
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "blog-service-kafka-cp-kafka-headless:9092",
                GroupId = "CommentService"
            };
            _topic = "Post.events";
            _kafkaConsumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        public void StartConsumerLoop(CancellationToken cancellationToken, Action<ReceivedEvent<TPayload>> onReceiveEvent)
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
                        onReceiveEvent(receivedEvent);
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

        private ReceivedEvent<TPayload> ProcessOutboxEvent(ConsumeResult<string, string> consumeResult)
        {
            var messageIdBuffer = consumeResult.Message.Headers.GetLastBytes("id");
            var id = System.Text.Encoding.UTF8.GetString(messageIdBuffer, 0, messageIdBuffer.Length);
            
            var eventTypeBuffer = consumeResult.Message.Headers.GetLastBytes("eventType");
            var type = System.Text.Encoding.UTF8.GetString(eventTypeBuffer, 0, eventTypeBuffer.Length);

            return new ReceivedEvent<TPayload>(id, type, JsonSerializer.Deserialize<TPayload>(consumeResult.Message.Value));
        }
        
    }
}

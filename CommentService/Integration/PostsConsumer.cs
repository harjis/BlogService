using System;
using System.Threading;
using CommentService.DAL;
using CommentService.Integration.Dto;
using Outbox.Consumer;
using Outbox.Consumer.Events;

namespace CommentService.Integration
{
    public class PostsConsumer
    {
        private readonly Builder<CommentDbContext, Post> _builder;


        public PostsConsumer(Builder<CommentDbContext, Post> builder)
        {
            _builder = builder;
        }

        public void StartConsumerLoop(CancellationToken cancellationToken)
        {
            _builder.StartConsumerLoop(cancellationToken, OnReceiveEvent);
        }

        public void Dispose()
        {
            _builder.Dispose();
        }

        private void OnReceiveEvent(ReceivedEvent<Post> receivedEvent)
        {
            Console.WriteLine($"CommentService.Integration.PostsBackgroundService has the event {receivedEvent.Id} {receivedEvent.Type}");
        }
    }
}

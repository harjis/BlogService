using System;
using System.Threading;
using System.Threading.Tasks;
using CommentService.Integration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommentService.BackgroundServices
{
    public class PostsBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public PostsBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            new Thread(() => StartConsumerLoop(stoppingToken)).Start();

            return Task.CompletedTask;
        }

        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var scopedPostsConsumer = scope.ServiceProvider.GetRequiredService<PostsConsumer>();
            scopedPostsConsumer.StartConsumerLoop(cancellationToken);
        }

        public override void Dispose()
        {
            using var scope = _serviceProvider.CreateScope();
            var scopedPostsConsumer = scope.ServiceProvider.GetRequiredService<PostsConsumer>();
            scopedPostsConsumer.Dispose();
            base.Dispose();
        }
    }
}

using System.Threading;
using System.Threading.Tasks;
using CommentService.Integration;
using Microsoft.Extensions.Hosting;

namespace CommentService.BackgroundServices
{
    public class PostsBackgroundService : BackgroundService
    {
        private readonly PostsConsumer _postsConsumer;
        public PostsBackgroundService(PostsConsumer postsConsumer)
        {
            _postsConsumer = postsConsumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            new Thread(() => StartConsumerLoop(stoppingToken)).Start();

            return Task.CompletedTask;
        }

        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            _postsConsumer.StartConsumerLoop(cancellationToken);
        }

        public override void Dispose()
        {
            _postsConsumer.Dispose();
            base.Dispose();
        }
    }
}

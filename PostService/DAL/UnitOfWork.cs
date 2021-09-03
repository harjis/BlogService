using System;
using System.Threading.Tasks;
using Outbox.Outbox;

namespace PostService.DAL
{
    public sealed class UnitOfWork : IDisposable
    {
        public readonly PostRepository PostRepository;
        public readonly OutboxManager<PostDbContext> OutboxManager;

        private bool _disposed;
        private readonly PostDbContext _dbContext;

        public UnitOfWork(PostDbContext dbContext, PostRepository postRepository, OutboxManager<PostDbContext> outboxManager)
        {
            _dbContext = dbContext;
            PostRepository = postRepository;
            OutboxManager = outboxManager;
        }

        public async Task ExecuteInTransaction(Func<Task> func)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await func();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Exception occured. Figure out what we should do here");
                throw;
            }
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            _disposed = true;
        }
    }
}

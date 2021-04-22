using System;
using System.Threading.Tasks;

namespace PostService.DAL
{
    public sealed class UnitOfWork : IDisposable
    {
        public readonly PostRepository PostRepository;

        private bool _disposed;
        private readonly PostDbContext _dbContext;

        public UnitOfWork(PostDbContext dbContext, PostRepository postRepository)
        {
            _dbContext = dbContext;
            PostRepository = postRepository;
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

using System.Threading.Tasks;
using PostService.DAL;
using PostService.Outbox.Events;

namespace PostService.Outbox
{
    public class OutboxManager
    {
        private readonly PostDbContext _dbContext;

        public OutboxManager(PostDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task FireEvent<T>(IOutboxEvent<T> outboxEvent)
        {
            var outbox = outboxEvent.ToOutboxModel();
            await _dbContext.AddAsync(outbox);
            await _dbContext.SaveChangesAsync();
            // TODO Then remove it after
        }
    }
}

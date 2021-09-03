using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Outbox.Outbox.Events;

namespace Outbox.Outbox
{
    public class OutboxManager<Context> where Context : DbContext
    {
        private readonly Context _dbContext;

        public OutboxManager(Context dbContext)
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

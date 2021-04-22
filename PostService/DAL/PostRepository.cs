using System;
using System.Threading.Tasks;
using PostService.Models;

namespace PostService.DAL
{
    public class PostRepository : GenericRepository<Post>
    {
        public PostRepository(PostDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task Add(Post entity)
        {
            await base.Add(entity);
            Console.WriteLine("Then create outbox");
        }
    }
}

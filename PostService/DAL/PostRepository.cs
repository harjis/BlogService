using PostService.Models;

namespace PostService.DAL
{
    public class PostRepository : GenericRepository<Post>
    {
        public PostRepository(PostDbContext dbContext) : base(dbContext)
        {
        }
    }
}

using PostService.Models;

namespace PostService.DAL
{
    public class PostRepository : GenericRepository<PostDbContext, Post>
    {
        public PostRepository(PostDbContext dbContext) : base(dbContext)
        {
        }
    }
}

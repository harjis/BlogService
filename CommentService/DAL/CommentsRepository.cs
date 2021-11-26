using CommentService.Models;

namespace CommentService.DAL
{
    public class CommentsRepository : GenericRepository<CommentDbContext, Comment>
    {
        public CommentsRepository(CommentDbContext dbContext) : base(dbContext)
        {
        }
    }
}

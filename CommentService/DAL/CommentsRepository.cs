using CommentService.Models;
using GenericRepository;

namespace CommentService.DAL
{
    public class CommentsRepository : GenericRepository<CommentDbContext, Comment>
    {
        public CommentsRepository(CommentDbContext dbContext) : base(dbContext)
        {
        }
    }
}

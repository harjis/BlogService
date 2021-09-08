using CommentService.DAL;
using CommentService.Integration;

namespace CommentService.Integration.Repositories
{
    public class PostRepository
    {
        private readonly CommentDbContext _dbContext;

        public PostRepository(CommentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Models.Post Add(Dto.Post post)
        {
            var model = DtoToModel(post);
            _dbContext.Posts.Add(model);
            _dbContext.SaveChanges();

            return model;
        }

        private Models.Post DtoToModel(Dto.Post post)
        {
            return new()
            {
                Id = post.Id,
                Title = post.Title
            };
        }
    }
}

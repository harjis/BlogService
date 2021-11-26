using CommentService.DAL;
using Microsoft.EntityFrameworkCore;

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

        public Models.Post Update(Dto.Post post)
        {
            // We need to get the existing model with GetById here so that EF Core fetches it either from db
            // or returns a attached model. If we would call DtoToModel here we would have 2 attached
            // models with the same Id and EF Core would throw an error:
            // The instance of entity type 'Post' cannot be tracked because another instance with the same key value for {'Id'}
            // TODO: Does this mean that the background tasks memory consumption goes up little by little because the
            // models are kept in memory after consumer has finished?
            var model = GetById(post.Id);
            model.Title = post.Title;
            _dbContext.Posts.Update(model);
            _dbContext.SaveChanges();

            return model;
        }

        public void Delete(object id)
        {
            var post = GetById(id);
            _dbContext.Posts.Remove(post);
            _dbContext.SaveChanges();
        }

        private Models.Post GetById(object id)
        {
            return _dbContext.Posts.Find(id);
        }

        private static Models.Post DtoToModel(Dto.Post post)
        {
            return new()
            {
                Id = post.Id,
                Title = post.Title
            };
        }
    }
}

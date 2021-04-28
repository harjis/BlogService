using System.Threading.Tasks;
using PostService.DAL;
using PostService.Events;
using PostService.Models;

namespace PostService.Services
{
    public class PostService
    {
        private readonly UnitOfWork _unitOfWork;

        public PostService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddPost(Post post)
        {
            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                await _unitOfWork.PostRepository.Add(post);
                await _unitOfWork.Save();
                await _unitOfWork.OutboxManager.FireEvent(new PostCreated(post));
            });
        }
    }
}
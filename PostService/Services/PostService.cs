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
                await _unitOfWork.EventManager.FireEvent(new PostCreated(post));
            });
        }

        public async Task UpdatePost(Post post)
        {
            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                _unitOfWork.PostRepository.Update(post);
                await _unitOfWork.Save();
                await _unitOfWork.EventManager.FireEvent(new PostUpdated(post));
            });
        }

        public async Task DeletePost(Post post)
        {
            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                _unitOfWork.PostRepository.Delete(post);
                await _unitOfWork.Save();
                await _unitOfWork.EventManager.FireEvent(new PostDeleted(post));
            });
        }
    }
}

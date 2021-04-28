using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostService.DAL;
using PostService.Models;

namespace PostService.Controllers
{
    public class PostsController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly Services.PostService _postService;

        public PostsController(UnitOfWork unitOfWork, Services.PostService postService)
        {
            _unitOfWork = unitOfWork;
            _postService = postService;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.PostRepository.Get());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _unitOfWork.PostRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content")] Post post)
        {
            if (ModelState.IsValid)
            {
                await _postService.AddPost(post);
                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _unitOfWork.PostRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.PostRepository.Update(post);
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _unitOfWork.PostRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _unitOfWork.PostRepository.Delete(id);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PostExists(int id)
        {
            return await _unitOfWork.PostRepository.Exists(id);
        }
    }
}

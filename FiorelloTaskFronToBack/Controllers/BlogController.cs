using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.ViewModels.Blog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace FiorelloTaskFronToBack.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = new BlogIndexViewModel
            {
                Blog = await _appDbContext.Blogs.ToListAsync()
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var blogtext = await _appDbContext.BlogTexts.FirstOrDefaultAsync(b => b.BlogId == id);
            var blog = await _appDbContext.Blogs.OrderByDescending(bl=>bl.Date).ToListAsync();
            var blogMainPhoto= await _appDbContext.Blogs.FirstOrDefaultAsync(ph=>ph.Id == id);
            var blogPhotos = await _appDbContext.BlogPhotos.Where(a => a.BlogId == id).ToListAsync();
            if (blogtext == null) return NotFound();
            var model = new BlogDetailsViewModel
            {
                Blogtext = blogtext,
                BlogPhotos = blogPhotos,
                Blog= blog,
                MainPhotoPath=blogMainPhoto.PhotoPath
            };
            return View(model);
        }
    }
}

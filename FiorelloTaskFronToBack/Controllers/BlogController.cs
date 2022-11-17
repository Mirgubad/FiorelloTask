using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.Models;
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
        public async Task<IActionResult> Index(BlogIndexViewModel model)
        {
            
            var blog = await PaginateBlogAsync(model.Page, model.Take);
            var pageCount = await GetPageCountAsync(model.Take);
            model = new BlogIndexViewModel
            {
                Blog = blog,
                PageCount = pageCount,
                Page=model.Page,

            };
            return View(model);
        }

        private async Task<List<Blog>> PaginateBlogAsync(int page,int take)
        {
            var blog = await _appDbContext.Blogs.Skip((page - 1) * take).Take(take).ToListAsync();
            return blog;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var pageCount = await _appDbContext.Blogs.CountAsync();
            return (int)Math.Ceiling((decimal)pageCount/take);
        }





        public async Task<IActionResult> Details(int id)
        {
            var blogtext = await _appDbContext.BlogTexts.FirstOrDefaultAsync(b => b.BlogId == id);
            var blog = await _appDbContext.Blogs.OrderByDescending(bl => bl.Date).ToListAsync();
            var blogMainPhoto = await _appDbContext.Blogs.FirstOrDefaultAsync(ph => ph.Id == id);
            var blogPhotos = await _appDbContext.BlogPhotos.Where(a => a.BlogId == id).ToListAsync();
            if (blogtext == null) return NotFound();
            var model = new BlogDetailsViewModel
            {
                Blogtext = blogtext,
                BlogPhotos = blogPhotos,
                Blog = blog,
                MainPhotoPath = blogMainPhoto.PhotoPath
            };
            return View(model);
        }
    }
}

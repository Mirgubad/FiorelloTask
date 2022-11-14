using FiorelloTaskFronToBack.Areas.Admin.ViewModels.Category;
using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.Helpers;
using FiorelloTaskFronToBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloTaskFronToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;

        private readonly IFileService _fileService;

        public CategoryController(AppDbContext appDbContext,
            IFileService fileService
            )
        {
            _appDbContext = appDbContext;

            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new CategoryIndexViewModel
            {
                categories = await _appDbContext.Categories.ToListAsync()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var dbcategory = await _appDbContext.Categories
                .AnyAsync(cc=>cc.Title.ToLower().Trim()==model.Title.ToLower().Trim());
            if (dbcategory)
            {
                ModelState.AddModelError("Title", "This category already in use");
                return View(model);
            }

            var category = new Category
            {
                Title = model.Title
            };
            await _appDbContext.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null) return NotFound();
            if (id != category.Id) return BadRequest();


            _appDbContext.Categories.Remove(category);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var model = new CategoryUpdateViewModel
            {
                Title = category.Title,

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryUpdateViewModel model)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (!ModelState.IsValid) return View(model);
            if (category == null) return NotFound();
            if (id != category.Id) return BadRequest();
            category.Title = model.Title;
            _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");

        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CategoryDetailsViewModel model)
        {

            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null) return NotFound();
            model.Title = category.Title;


            return View(model);
        }
    }
}

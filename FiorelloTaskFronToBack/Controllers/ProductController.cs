using FiorelloTaskFronToBack.ViewModels.Product;
using FiorelloTaskFronToBack.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloTaskFronToBack.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ProductController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = new ProductIndexViewModel
            {
                Products = await _appDbContext.Products
                .OrderByDescending(p => p.Id)
                .Take(4)
                .ToListAsync(),
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dbProduct = await _appDbContext.Products
                .Include(p => p.ProductPhotos)
                .Include(ct => ct.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dbProduct == null) return NotFound();


            var model = new ProductDetailsViewModel
            {
                Title = dbProduct.Title,
                CategoryId = dbProduct.CategoryId,
                Description = dbProduct.Description,
                Cost = dbProduct.Cost,
                Quantity = dbProduct.Quantity,
                Status = dbProduct.Status,
                Weight = dbProduct.Weight,
                Category = dbProduct.Category,
                ProductPhotos = dbProduct.ProductPhotos,
                MainPhotoPath = dbProduct.MainPhotoPath
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadMore(int skipRow)
        {
            var products = await _appDbContext.Products
                .OrderByDescending(p => p.Id)
                .Skip(4 * skipRow)
                .Take(4)
                .ToListAsync();
            bool isLast = false;
            if (((skipRow + 1) * 4) + 1 >= _appDbContext.Products.Count())
            {
                isLast = true;
            }
            var model = new ProductLoadMoreViewModel
            {
                Products = products,
                IsLast = isLast
            };
            return PartialView("_ProductPartial", model);
        }
    }
}

using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.Models;
using FiorelloTaskFronToBack.ViewModels.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloTaskFronToBack.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly UserManager<Models.User> _userManager;
        private readonly AppDbContext _context;

        public BasketController(UserManager<Models.User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var basket = await _context.Baskets
                .Include(b => b.BasketProducts)
                .ThenInclude(bp => bp.Product)
                .FirstOrDefaultAsync(b => b.UserId == user.Id);

            var model = new BasketIndexViewModel();

            if (basket == null)
            {
                List<BasketAddViewModel> basketAddViewModels = new List<BasketAddViewModel>();
                return View(model);
            }
            foreach (var dbbasketProduct in basket.BasketProducts)
            {
                var basketProduct = new BasketProductViewModel
                {
                    Id = dbbasketProduct.ProductId,
                    Title = dbbasketProduct.Product.Title,
                    PhotoName = dbbasketProduct.Product.MainPhotoPath,
                    Price = dbbasketProduct.Product.Cost,
                    StockQuantity = dbbasketProduct.Quantity,
                    Quantity = dbbasketProduct.Quantity,
                };
                model.BasketProducts.Add(basketProduct);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BasketAddViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var product = await _context.Products.FindAsync(model.Id);
            if (product == null) return NotFound();

            var basket = await _context.Baskets.FirstOrDefaultAsync(b => b.UserId == user.Id);

            if (basket == null)
            {
                basket = new Basket
                {
                    UserId = user.Id
                };
                await _context.Baskets.AddAsync(basket);
                await _context.SaveChangesAsync();
            }

            var basketProduct = await _context.BasketProducts.FirstOrDefaultAsync(bp => bp.ProductId == model.Id && bp.BasketId == basket.Id);

            if (basketProduct != null)
            {
                basketProduct.Quantity++;
            }
            else
            {
                basketProduct = new BasketProduct
                {
                    BasketId = basket.Id,
                    ProductId = product.Id,
                    Quantity = 1
                };
                await _context.BasketProducts.AddAsync(basketProduct);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var basketProduct = await _context.BasketProducts.FirstOrDefaultAsync(bp => bp.ProductId == id && bp.Basket.UserId == user.Id);
            if (basketProduct == null) return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketProduct.ProductId);
            if (product == null) return NotFound();

            _context.BasketProducts.Remove(basketProduct);
            await _context.SaveChangesAsync();

            return Ok();
        }



        [HttpPost]
        public async Task<IActionResult> Less(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var basket = await _context.Baskets
                .Include(bp => bp.BasketProducts)
                .FirstOrDefaultAsync(b => b.UserId == user.Id);

            if (basket != null)
            {
                foreach (var dbbasketProduct in basket.BasketProducts)
                {
                    if (dbbasketProduct.ProductId == id)
                    {
                        dbbasketProduct.Quantity--;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> More(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var basket = await _context.Baskets
                .Include(bp => bp.BasketProducts)
                .FirstOrDefaultAsync(b => b.UserId == user.Id);

            if (basket != null)
            {
                foreach (var dbbasketProduct in basket.BasketProducts)
                {
                    if (dbbasketProduct.ProductId == id)
                    {
                        dbbasketProduct.Quantity++;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return Ok();

        }

    }
}

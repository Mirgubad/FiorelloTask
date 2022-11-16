using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.ViewModels.Basket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using NuGet.ContentModel;

namespace FiorelloTaskFronToBack.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketAddViewModel> basketProducts;

            if (Request.Cookies["basket"] != null)
            {
                basketProducts = JsonConvert.DeserializeObject<List<BasketAddViewModel>>(Request.Cookies["basket"]);
            }
            else
            {
                basketProducts = new List<BasketAddViewModel>();
            }


          

            List<BasketListitemViewModel> model = new List<BasketListitemViewModel>();

            foreach (var basketProduct in basketProducts)
            {
                var dbproduct = await _context.Products.FindAsync(basketProduct.Id);

                if (dbproduct != null)
                {
                    model.Add(new BasketListitemViewModel
                    {
                        Id = dbproduct.Id,
                        Photoname = dbproduct.MainPhotoPath,
                        Price = dbproduct.Cost,
                        Quantity = basketProduct.Count,
                        StockQuantity = dbproduct.Quantity,
                        Title = dbproduct.Title,
                    });
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BasketAddViewModel model)
        {
            List<BasketAddViewModel> basket;


            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketAddViewModel>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketAddViewModel>();
            }

            var basketProduct = basket.Find(b => b.Id == model.Id);
            if (basketProduct != null)
            {
                basketProduct.Count++;
            }
            else
            {
                model.Count++;
                basket.Add(model);
            }

            var serializedBasket = JsonConvert.SerializeObject(basket);
            Response.Cookies.Append("basket", serializedBasket);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Less(int id)
        {
            var basket = JsonConvert.DeserializeObject<List<BasketAddViewModel>>(Request.Cookies["basket"]);

            var basketProduct = basket.Find(b => b.Id == id);
            if (basketProduct != null)
            {
                basketProduct.Count--;

            }

            var serializedBasket = JsonConvert.SerializeObject(basket);
            Response.Cookies.Append("basket", serializedBasket);

            return Ok();


        }

        [HttpPost]
        public async Task<IActionResult> More(int id)
        {
            var basket = JsonConvert.DeserializeObject<List<BasketAddViewModel>>(Request.Cookies["basket"]);

            var basketProduct = basket.Find(b => b.Id == id);
            if (basketProduct != null)
            {
                basketProduct.Count++;

            }
            var serializedBasket = JsonConvert.SerializeObject(basket);
            Response.Cookies.Append("basket", serializedBasket);

            return Ok();

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            List<BasketAddViewModel> basket;

            if (Request.Cookies["basket"] == null) return NotFound();

            basket = JsonConvert.DeserializeObject<List<BasketAddViewModel>>(Request.Cookies["basket"]);

            var dbProduct = await _context.Products.FindAsync(id);

            if (dbProduct == null) return NotFound();

            var basketProduct = basket.Find(p => p.Id == id);

            if (basketProduct != null)
            {
                basket.Remove(basketProduct);
            }

            var serializedBasket = JsonConvert.SerializeObject(basket);

            Response.Cookies.Append("basket", serializedBasket);

            return Ok();
        }
    }
}

using FiorelloTaskFronToBack.Areas.Admin.ViewModels.FaqPage;
using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.Migrations;
using FiorelloTaskFronToBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace FiorelloTaskFronToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FaqPageController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public FaqPageController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }
        public async Task<IActionResult> Index()
        {
            var model = new FaqPageIndexViewModel
            {
                FaqComponents = await _appDbContext.FaqComponents.ToListAsync()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FaqPageCreateViewModel model)
        {

            if (!ModelState.IsValid) return View(model);
            bool isExist = await _appDbContext.FaqComponents
                .AnyAsync(t => t.Title.ToLower().Trim() == model.Title.ToLower().Trim());

            if (isExist)
            {
                ModelState.AddModelError("Title", "This faq already created ");
                return View(model);
            }
            var dbfaq = await _appDbContext.FaqComponents.ToListAsync();

            var order = dbfaq.Count;
            if (dbfaq.Count == 0)
            {
                order = 1;
            }
            else
            {
                order++;
            }
            var faqcomponent = new FaqComponent
            {
                Title = model.Title,
                Description = model.Description,
                Order = order,
            };

            await _appDbContext.FaqComponents.AddAsync(faqcomponent);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var faqcomponent = await _appDbContext.FaqComponents.FindAsync(id);
            if (faqcomponent == null) return NotFound();

            var model = new FaqComponentUpdateViewModel
            {
                Title = faqcomponent.Title,
                Description = faqcomponent.Description,

            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, FaqComponentUpdateViewModel model)
        {
            var faqcomponent = await _appDbContext.FaqComponents.FindAsync(id);
            if (faqcomponent == null) return NotFound();
            faqcomponent.Title = model.Title;
            faqcomponent.Description = model.Description;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var faqcomponent = await _appDbContext.FaqComponents.FindAsync(id);
            if (faqcomponent == null) return NotFound();

            var model = new FaqComponentDetailsViewModel
            {

                Id = faqcomponent.Id,
                Title = faqcomponent.Title,
                Description = faqcomponent.Description,
                Order = faqcomponent.Order,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var faqcomponent = await _appDbContext.FaqComponents.FindAsync(id);
            if (faqcomponent == null) return NotFound();

            _appDbContext.FaqComponents.Remove(faqcomponent);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            var dbfaq = await _appDbContext.FaqComponents.FindAsync(id);
            if (dbfaq == null) return NotFound();
            var model = new FaqPageUpdateOrderViewModel
            {
                Order = dbfaq.Order,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(int id, FaqPageUpdateOrderViewModel model)
        {
            var dbfaq = await _appDbContext.FaqComponents.FindAsync(id);
            if (dbfaq == null) return NotFound();
            if (!ModelState.IsValid) return View(model);
            dbfaq.Order = model.Order;
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("details", "faqpage", new {id=dbfaq.Id});

        }
    }
}

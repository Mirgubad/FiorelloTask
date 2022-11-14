using FiorelloTaskFronToBack.Areas.Admin.ViewModels.Testimonial;
using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace FiorelloTaskFronToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TestimonialController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public TestimonialController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = new TestimonialIndexViewModel
            {
                Testimonial = await _appDbContext.Testimonials.Include(fe => fe.FlowerExpert).ToListAsync()
            };
            return View(model);
        }

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new TestimonialCreateViewModel
            {
                FlowerExpert = await _appDbContext.FlowerExperts.Select(fe => new SelectListItem
                {
                    Value = fe.Id.ToString(),
                    Text = fe.Name + " " + fe.Surname,
                }
                ).ToListAsync(),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestimonialCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            int order = 1;
            var testimonial = new Testimonial
            {
                Title = model.Title,
                FlowerExpertId = model.FlowerExpertId,
                Order = ++order,
            };
            await _appDbContext.Testimonials.AddAsync(testimonial);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }
        #endregion

        #region Update

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var testimonial = await _appDbContext.Testimonials.FindAsync(id);
            if (testimonial == null) return NotFound();

            var model = new TestimonialUpdateViewModel
            {
                FlowerExpertId = testimonial.FlowerExpertId,
                Title = testimonial.Title,
                FlowerExpert = await _appDbContext.FlowerExperts.Select(fe => new SelectListItem
                {
                    Value = fe.Id.ToString(),
                    Text = fe.Name + " " + fe.Surname
                }).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, TestimonialUpdateViewModel model)
        {
            var testimonial = await _appDbContext.Testimonials.FindAsync(id);
            if (testimonial == null) return NotFound();
            model.FlowerExpert = await _appDbContext.FlowerExperts.Select(fe => new SelectListItem
            {
                Value = fe.Id.ToString(),
                Text = fe.Name + " " + fe.Surname,
            }).ToListAsync();
            if (!ModelState.IsValid) return View(model);
            testimonial.Title = model.Title;
            testimonial.FlowerExpertId = model.FlowerExpertId;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var testimonial = await _appDbContext.Testimonials.FindAsync(id);
            if (testimonial == null) return NotFound();
            var expert = await _appDbContext.FlowerExperts.Where(fe => fe.Id == testimonial.FlowerExpertId).FirstOrDefaultAsync();
            var model = new TestimonialDetailsViewModel
            {
                Id=testimonial.Id,
                FlowerExpertId=testimonial.FlowerExpertId,
                Title=testimonial.Title,    
                FlowerExpert=expert
            };
            return View(model);
        }

        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbtestimonial = await _appDbContext.Testimonials.FindAsync(id);
            if (dbtestimonial == null) return NotFound();

            _appDbContext.Testimonials.Remove(dbtestimonial);

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");

        }
        #endregion
    }

}

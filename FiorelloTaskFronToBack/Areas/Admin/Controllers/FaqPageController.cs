﻿using FiorelloTaskFronToBack.Areas.Admin.ViewModels.FaqPage;
using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace FiorelloTaskFronToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
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

            var faqcomponent = new FaqComponent
            {
                Title = model.Title,
                Description = model.Description
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
                Description=faqcomponent.Description,   

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

                Id=faqcomponent.Id,
                Title=faqcomponent.Title,
                Description=faqcomponent.Description
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
    }
}

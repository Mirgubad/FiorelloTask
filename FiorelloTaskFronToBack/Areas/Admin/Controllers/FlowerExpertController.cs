using FiorelloTaskFronToBack.Areas.Admin.ViewModels.FlowerExpert;
using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.Helpers;
using FiorelloTaskFronToBack.Migrations;
using FiorelloTaskFronToBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloTaskFronToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FlowerExpertController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public FlowerExpertController(AppDbContext appDbContext,
            IWebHostEnvironment webHostEnvironment,
            IFileService fileService)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new FlowerExpertIndexViewModel
            {
                FlowerExpert = await _appDbContext.FlowerExperts.ToListAsync()
            };
            return View(model);
        }

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FlowerExpertCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_fileService.CheckPhoto(model.Photo))
            {
                ModelState.AddModelError("Photo", "File format must be image");
                return View(model);
            }
            int maxSize = 1000;
            if (!_fileService.MaxSize(model.Photo, maxSize))
            {
                ModelState.AddModelError("Photo", $"Photo size must be less{maxSize}");
                return View(model);
            }
            var flowerexpert = new FlowerExpert
            {
                Name = model.Name,
                Surname = model.Surname,
                Position = model.Position,
                PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath)
            };
            await _appDbContext.FlowerExperts.AddAsync(flowerexpert);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var dbexpert = await _appDbContext.FlowerExperts.FindAsync(id);
            if (dbexpert == null) return NotFound();

            var model = new FlowerExpertUpdateViewModel
            {
                Name = dbexpert.Name,
                Surname = dbexpert.Surname,
                Position = dbexpert.Position,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, FlowerExpertUpdateViewModel model)
        {
            var dbexpert = await _appDbContext.FlowerExperts.FindAsync(id);
            if (dbexpert == null) return NotFound();

            if (!ModelState.IsValid) return View(model);
            dbexpert.Name = model.Name;
            dbexpert.Surname = model.Surname;
            dbexpert.Position = model.Position;
            await _appDbContext.SaveChangesAsync();
            if (model.Photo != null)
            {
                dbexpert.PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
                await _appDbContext.SaveChangesAsync();
            }
            return RedirectToAction("index");
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dbexpert = await _appDbContext.FlowerExperts.FindAsync(id);
            if (dbexpert == null) return NotFound();

            var model = new FlowerExpertDetailsViewModel
            {
                Id = dbexpert.Id,
                Name = dbexpert.Name,
                Surname = dbexpert.Surname,
                Position = dbexpert.Position,
                PhotoPath = dbexpert.PhotoPath,
            };

            return View(model);
        }
        #endregion

        #region Delete

        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            var dbexpert = await _appDbContext.FlowerExperts.FindAsync(id);
            if (dbexpert == null) return NotFound();

            _appDbContext.FlowerExperts.Remove(dbexpert);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }
        #endregion
    }
}

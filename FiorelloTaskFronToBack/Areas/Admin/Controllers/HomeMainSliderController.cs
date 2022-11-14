using FiorelloTaskFronToBack.Areas.Admin.ViewModels.HomeMainSlider;
using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.Helpers;
using FiorelloTaskFronToBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FiorelloTaskFronToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeMainSliderController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public HomeMainSliderController(AppDbContext appDbContext,
            IWebHostEnvironment webHostEnvironment, IFileService fileService
            )
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new HomeMainSliderIndexViewModel
            {
                HomeMainSlider = await _appDbContext.HomeMainSlider.FirstOrDefaultAsync()
            };
            return View(model);
        }


        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var dbhomemainslider = await _appDbContext.HomeMainSlider.FirstOrDefaultAsync();
            if (dbhomemainslider != null) return NotFound();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HomeMainSliderCreateViewModel model)
        {
            var dbhomeslider = await _appDbContext.HomeMainSlider.FirstOrDefaultAsync();
            if (dbhomeslider != null) return NotFound();
            if (!ModelState.IsValid) return View(model);

            int maxSize = 1000;
            if (!_fileService.CheckPhoto(model.SubPhoto))
            {
                ModelState.AddModelError("SubPhoto", "File type was incorrect");
                return View(model);
            }
            if (!_fileService.MaxSize(model.SubPhoto, maxSize))
            {
                ModelState.AddModelError("Subphoto", $"File size must be less{maxSize}kb");
                return View(model);
            }
            var homemainslider = new HomeMainSlider
            {
                Description = model.Description,
                Title = model.Title,
                SubPhotoName = await _fileService.UploadAsync(model.SubPhoto, _webHostEnvironment.WebRootPath),
            };

            await _appDbContext.HomeMainSlider.AddAsync(homemainslider);
            await _appDbContext.SaveChangesAsync();

            int order = 1;
            bool hasError = false;
            foreach (var sliderPhoto in model.SliderPhotos)
            {
                if (!_fileService.CheckPhoto(sliderPhoto))
                {
                    ModelState.AddModelError("SliderPhoto", "File type was incorrect");
                    hasError = true;
                }
                if (!_fileService.MaxSize(sliderPhoto, maxSize))
                {
                    ModelState.AddModelError("SliderPhoto", $"File size must be less {maxSize}");
                    hasError = true;
                }
                if (hasError)
                {
                    return View(model);
                }
                var homemainsliderPhotos = new HomeMainSliderPhoto
                {
                    HomeMainSliderId = homemainslider.Id,
                    Name = await _fileService.UploadAsync(sliderPhoto, _webHostEnvironment.WebRootPath),
                    Order = order,
                };

                order++;
                await _appDbContext.HomeMainSliderPhotos.AddAsync(homemainsliderPhotos);
                await _appDbContext.SaveChangesAsync();
            }
            return RedirectToAction("index");
        }
        #endregion

        #region Update

        public async Task<IActionResult> Update()
        {
            var dbhomemainslider = await _appDbContext.HomeMainSlider.FirstOrDefaultAsync();
            if (dbhomemainslider == null) return NotFound();
            var model = new HomeMainSliderUpdateViewModel
            {
                Title = dbhomemainslider.Title,
                Description = dbhomemainslider.Description,
                HomeMainSliderPhotos = dbhomemainslider.HomeMainSliderPhotos,
            };
            return View(model);
        }


        [HttpPost]

        public async Task<IActionResult> Update(HomeMainSliderUpdateViewModel model)
        {
            var dbhomemainslider = await _appDbContext.HomeMainSlider.Include(mp => mp.HomeMainSliderPhotos).FirstOrDefaultAsync();
            if (dbhomemainslider == null) return NotFound();

            if (!ModelState.IsValid) return View(model);
            dbhomemainslider.Title = model.Title;
            dbhomemainslider.Description = model.Description;
            await _appDbContext.SaveChangesAsync();
            int maxSize = 1000;
            if (model.SubPhoto != null)
            {
                if (!_fileService.CheckPhoto(model.SubPhoto))
                {
                    ModelState.AddModelError("SubPhoto", "File was incorrect type");
                    return View(model);
                }
                else if (!_fileService.MaxSize(model.SubPhoto, maxSize))
                {
                    ModelState.AddModelError("SubPhoto", $"Photo size must be less than {maxSize}");
                    return View(model);
                }

                dbhomemainslider.SubPhotoName = await _fileService.UploadAsync(model.SubPhoto, _webHostEnvironment.WebRootPath);
                await _appDbContext.SaveChangesAsync();
            }


            int Order = dbhomemainslider.HomeMainSliderPhotos.Count + 1;
            bool hasError = false;
            if (model.SliderPhotos != null)
            {
                foreach (var sliderPhoto in model.SliderPhotos)
                {
                    if (!_fileService.CheckPhoto(sliderPhoto))
                    {
                        ModelState.AddModelError("SliderPhoto", "File was incorrect typw");
                        hasError = true;
                    }
                    else if (!_fileService.MaxSize(sliderPhoto, maxSize))
                    {
                        ModelState.AddModelError("SliderPhoto", $"File size must be less {maxSize} kb");
                        hasError = true;
                    }
                    if (hasError)
                    {
                        return View(model);
                    }

                    var sliderPhotos = new HomeMainSliderPhoto
                    {
                        Name = await _fileService.UploadAsync(sliderPhoto, _webHostEnvironment.WebRootPath),
                        HomeMainSliderId = dbhomemainslider.Id,
                        Order = Order
                    };
                    await _appDbContext.HomeMainSliderPhotos.AddAsync(sliderPhotos);
                    await _appDbContext.SaveChangesAsync();
                    Order++;
                }
            }
            return RedirectToAction("index");
        }
        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dbhomeMainSlider = await _appDbContext.HomeMainSlider
                .Include(sl => sl.HomeMainSliderPhotos).FirstOrDefaultAsync(x => x.Id == id);
            if (dbhomeMainSlider == null) return NotFound();
            var model = new HomeMainSliderDetailsViewModel
            {
                Title = dbhomeMainSlider.Title,
                Description = dbhomeMainSlider.Description,
                Id = dbhomeMainSlider.Id,
                HomeMainSliderPhotos = dbhomeMainSlider.HomeMainSliderPhotos,
                SubPhotoPath = dbhomeMainSlider.SubPhotoName
            };
            return View(model);
        }
        #endregion

        #region Delete

        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var dbhomemainslider = await _appDbContext.HomeMainSlider.FirstOrDefaultAsync();
            if (dbhomemainslider == null) return NotFound();
            _appDbContext.HomeMainSlider.Remove(dbhomemainslider);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }
        #endregion

        #region UpdatePhoto
        [HttpGet]
        public async Task<IActionResult> UpdatePhoto(int id)
        {
            var dbmainphotoSlider = await _appDbContext.HomeMainSliderPhotos.FindAsync(id);
            if (dbmainphotoSlider == null) return NotFound();
            var model = new HomeMainSliderUpdatePhotoViewModel
            {
                Order = dbmainphotoSlider.Order
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePhoto(int id, HomeMainSliderUpdatePhotoViewModel model)
        {

            var dbHomeMainSliderPhoto = await _appDbContext.HomeMainSliderPhotos.FindAsync(id);
            if (dbHomeMainSliderPhoto == null) return NotFound();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Order", "Please fill the data");
                return View(dbHomeMainSliderPhoto);
            }
            dbHomeMainSliderPhoto.Order = model.Order;
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("details", "homemainslider", new { id = dbHomeMainSliderPhoto.HomeMainSliderId });

        }
        #endregion

        #region DeletePhoto

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var dbhomeMainSliderPhoto = await _appDbContext.HomeMainSliderPhotos.FindAsync(id);
            if (dbhomeMainSliderPhoto == null) return NotFound();

            _appDbContext.HomeMainSliderPhotos.Remove(dbhomeMainSliderPhoto);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("details", "homemainslider", new { id = dbhomeMainSliderPhoto.HomeMainSliderId });

        }
        #endregion
    }
}

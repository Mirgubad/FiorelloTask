using FiorelloTaskFronToBack.Areas.Admin.ViewModels.Product;
using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.Helpers;
using FiorelloTaskFronToBack.Migrations;
using FiorelloTaskFronToBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.EntityFrameworkCore;

namespace FiorelloTaskFronToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public ProductController(AppDbContext appDbContext,
            IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync();
            if (product == null) return NotFound();
            var model = new ProductIndexViewModel
            {
                Product = await _appDbContext.Products
                .Include(c => c.Category)
                .ToListAsync()
            };
            return View(model);
        }

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var product = await _appDbContext.Products.Include(pt => pt.ProductPhotos).FirstOrDefaultAsync();
            var model = new ProductCreateViewModel
            {
                Categories = await _appDbContext.Categories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                }).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            model.Categories = await _appDbContext.Categories.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString()
            }).ToListAsync();
            if (!ModelState.IsValid) return View(model);
            var product = new Product
            {
                Cost = model.Cost,
                Description = model.Description,
                Quantity = model.Quantity,
                Status = model.Status,
                Title = model.Title,
                Weight = model.Weight,
                CategoryId = model.CategoryId,
                MainPhotoPath = model.MainPhoto.Name,
                ProductPhotos = model.ProductPhotos
            };
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            bool hasError = false;
            int maxSize = 1000;
            if (!_fileService.CheckPhoto(model.MainPhoto))
            {
                ModelState.AddModelError("MainPhoto", $"File must be Image");
                return View(model);
            }
            if (!_fileService.MaxSize(model.MainPhoto, maxSize))
            {
                ModelState.AddModelError("MainPhoto", $"Photo size must be less{maxSize} ");
                return View(model);
            }
            foreach (var photo in model.Photos)
            {
                if (!_fileService.CheckPhoto(photo))
                {
                    ModelState.AddModelError("Photos", $"File must be Image");
                }
                else if (!_fileService.MaxSize(photo, maxSize))
                {
                    ModelState.AddModelError("MainPhoto", $"Photo size must be less{maxSize} ");
                }
            }
            if (hasError)
            {
                return View(model);
            }
            else
            {
                product.MainPhotoPath = await _fileService.UploadAsync(model.MainPhoto, _webHostEnvironment.WebRootPath);
                await _appDbContext.SaveChangesAsync();
            }
            int order = 1;
            foreach (var photo in model.Photos)
            {
                var productPhotos = new ProductPhoto
                {
                    Name = await _fileService.UploadAsync(photo, _webHostEnvironment.WebRootPath),
                    Order = order,
                    ProductId = product.Id
                };
                order++;
                await _appDbContext.AddAsync(productPhotos);
                await _appDbContext.SaveChangesAsync();
            }
            return RedirectToAction("index");
        }

        #endregion

        #region Update

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var dbproduct = await _appDbContext.Products
               .Include(pp => pp.ProductPhotos)
               .FirstOrDefaultAsync(p=>p.Id==id);
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null) return NotFound();

            var model = new ProductUpdateViewModel
            {
                Title = product.Title,
                Description = product.Description,
                Status = product.Status,
                CategoryId = product.CategoryId,
                Cost = product.Cost,
                Quantity = product.Quantity,
                Weight = product.Weight,
                Categories = await _appDbContext.Categories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                }).ToListAsync(),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, ProductUpdateViewModel model)
        {
            var product = await _appDbContext.Products
           .Include(p => p.ProductPhotos)
           .FirstOrDefaultAsync(x=>x.Id==id);

            bool isExists = await _appDbContext.Products.AnyAsync(p => p.Title.ToLower().Trim() == model.Title.ToLower().Trim()
            && model.Id != p.Id);

            if (isExists)
            {
                ModelState.AddModelError("Title", "This product already have");
                return View(model);
            }
            model.Categories = await _appDbContext.Categories
                .Select(pt => new SelectListItem
                {
                    Text = pt.Title,
                    Value = pt.Id.ToString()

                }).ToListAsync();


            var dbproduct = await _appDbContext.Products.FindAsync(id);
            if (dbproduct == null) return NotFound();
            if (model.Id != dbproduct.Id) return BadRequest();


            bool hasError = false;
            int maxSize = 1000;

            if (model.Photos != null)
            {
                foreach (var photo in model.Photos)
                {
                    if (!_fileService.CheckPhoto(photo))
                    {
                        ModelState.AddModelError("Photos", "File must be image");
                        hasError = true;
                    }
                    else if (!_fileService.MaxSize(photo, maxSize))
                    {
                        ModelState.AddModelError("Photos", $"Photo size must be less {maxSize}kb");
                        hasError = true;
                    }


                 
                        int order = 1;
                        var productPhoto = new ProductPhoto
                        {
                            Name = await _fileService.UploadAsync(photo, _webHostEnvironment.WebRootPath),
                            Order = order,
                            ProductId = product.Id
                        };
                        await _appDbContext.ProductPhotos.AddAsync(productPhoto);
                        await _appDbContext.SaveChangesAsync();
                        order++;
                    
                }
                if (hasError)
                {
                    return View(model);
                }
              
            }

            if (model.MainPhoto != null)
            {
                if (!_fileService.CheckPhoto(model.MainPhoto))
                {
                    ModelState.AddModelError("Photos", "File must be image");
                    hasError = true;
                }
                else if (!_fileService.MaxSize(model.MainPhoto, maxSize))
                {
                    ModelState.AddModelError("Photos", $"Photo size must be less {maxSize}kb");
                    hasError = true;
                }
                dbproduct.MainPhotoPath = await _fileService.UploadAsync(model.MainPhoto, _webHostEnvironment.WebRootPath);
                await _appDbContext.SaveChangesAsync();
            }
            if (hasError)
            {
                return View(model);
            }

            if (!ModelState.IsValid) return View(model);
            dbproduct.Title = model.Title;
            dbproduct.Description = model.Description;
            dbproduct.Status = model.Status;
            dbproduct.CategoryId = model.CategoryId;
            dbproduct.Cost = model.Cost;
            dbproduct.Quantity = model.Quantity;
            dbproduct.Weight = model.Weight;

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }
        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _appDbContext.Products
          .Include(p => p.ProductPhotos)
          .FirstOrDefaultAsync(p => p.Id == id);
            var dbproduct = await _appDbContext.Products.FindAsync(id);
            if (dbproduct == null) return NotFound();
            var model = new ProductDetailsViewModel
            {
                Id = dbproduct.Id,
                Title = dbproduct.Title,
                Description = dbproduct.Description,
                Status = dbproduct.Status,
                CategoryId = dbproduct.CategoryId,
                Cost = dbproduct.Cost,
                Quantity = dbproduct.Quantity,
                Weight = dbproduct.Weight,
                Categories = await _appDbContext.Categories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                }).ToListAsync(),
                PhotoPath = dbproduct.MainPhotoPath,
                ProductPhotos = dbproduct.ProductPhotos

            };
            return View(model);
        }
        #endregion

        #region Delete

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbproduct = await _appDbContext.Products.Include(p => p.ProductPhotos).FirstOrDefaultAsync(mp => mp.Id == id);
            if (dbproduct == null) return NotFound();
            if (id != dbproduct.Id) return BadRequest();

            _appDbContext.Products.Remove(dbproduct);
            foreach (var photo in dbproduct.ProductPhotos)
            {
                _fileService.Delete(_webHostEnvironment.WebRootPath, photo.Name);
                await _appDbContext.SaveChangesAsync();
            }
            _fileService.Delete(_webHostEnvironment.WebRootPath, dbproduct.MainPhotoPath);
            _appDbContext.Products.Remove(dbproduct);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion



        #region DeletePhotos

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photo = await _appDbContext.ProductPhotos.FindAsync(id);
            if (photo == null) return NotFound();

            _fileService.Delete(_webHostEnvironment.WebRootPath, photo.Name);
            _appDbContext.ProductPhotos.Remove(photo);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("details", "product", new { Id = photo.ProductId });

        }

        #endregion

        #region UpdatePhoto

        [HttpGet]
        public async Task<IActionResult> UpdatePhoto(int id)
        {
            var dbPhoto = await _appDbContext.ProductPhotos.FindAsync(id);
            if (dbPhoto == null) return NotFound();
            var model = new ProductPhotoUpdateViewModel
            {
                Id = dbPhoto.Id,
                Order = dbPhoto.Order,
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePhoto(int id, ProductPhotoUpdateViewModel model)
        {
            var dbPhoto = await _appDbContext.ProductPhotos.FindAsync(id);
            if (dbPhoto == null) return NotFound();
            dbPhoto.Order = model.Order;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");

        }

        #endregion


    }
}

using FiorelloTaskFronToBack.Areas.Admin.ViewModels.Blog;
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
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public BlogController(AppDbContext appDbContext,
            IWebHostEnvironment webHostEnvironment,
            IFileService fileService)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new BlogIndexViewModel
            {
                Blogs = await _appDbContext.Blogs.ToListAsync()
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
        public async Task<IActionResult> Create(BlogCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            bool isExists = await _appDbContext.Blogs.AnyAsync(b => b.Title == model.Title && b.Id == model.Id);
            if (isExists)
            {
                ModelState.AddModelError("Title", "This blogname already created");
                return View(model);
            }
            int maxSize = 1000;

            if (!_fileService.CheckPhoto(model.Photo))
            {
                ModelState.AddModelError("Photo", "File type must be image");
                return View(model);
            }
            if (!_fileService.MaxSize(model.Photo, maxSize))
            {
                ModelState.AddModelError("Photo", $"{model.Photo} size must be less than {maxSize} kb");
                return View(model);
            }
            var blog = new Blog
            {
                Title = model.Title,
                Description = model.Description,
                Date = model.Date,
                PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath),
            };
            await _appDbContext.Blogs.AddAsync(blog);
            await _appDbContext.SaveChangesAsync();
            var blogtext = new BlogText
            {
                Title = model.BlogText.Title,
                DescriptionHead = model.BlogText.DescriptionHead,
                DescriptionEnd = model.BlogText.DescriptionEnd,
                BlogId = blog.Id
            };
            await _appDbContext.BlogTexts.AddAsync(blogtext);
            await _appDbContext.SaveChangesAsync();
            bool hasError = false;
            foreach (var blogcomponentphoto in model.BlogPhotos)
            {
                if (!_fileService.CheckPhoto(blogcomponentphoto))
                {
                    ModelState.AddModelError("BlogPhotos", "File type must be image");
                    hasError = true;
                }
                else if (!_fileService.MaxSize(blogcomponentphoto, maxSize))
                {
                    ModelState.AddModelError("BlogPhotos", $"{blogcomponentphoto.Name} size must be less than {maxSize} kb");
                    hasError = true;
                }

                var blogcopmonent = new BlogPhoto
                {
                    BlogId = blog.Id,
                    PhotoPath = await _fileService.UploadAsync(blogcomponentphoto, _webHostEnvironment.WebRootPath)
                };
                await _appDbContext.AddAsync(blogcopmonent);
                await _appDbContext.SaveChangesAsync();
            }
            if (hasError)
            {
                return View(model);
            }
            return RedirectToAction("index");
        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var dbBlog = await _appDbContext.Blogs.Include(bt => bt.BlogText).Include(bp => bp.BlogPhotos).FirstOrDefaultAsync(b => b.Id == id);
            if (dbBlog == null) return NotFound();
            var model = new BlogUpdateViewModel
            {
                Title = dbBlog.Title,
                Description = dbBlog.Description,
                Date = dbBlog.Date,
                BlogText = dbBlog.BlogText
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, BlogUpdateViewModel model)
        {
            var dbBlog = await _appDbContext.Blogs.Include(bt => bt.BlogText).Include(bp => bp.BlogPhotos).FirstOrDefaultAsync(b => b.Id == id);
            if (dbBlog == null) return NotFound();
            if (!ModelState.IsValid) return View(model);
            dbBlog.Title = model.Title;
            dbBlog.Description = model.Description;
            dbBlog.Date = (DateTime)model.Date;
            dbBlog.BlogText.DescriptionEnd = model.BlogText.DescriptionEnd;
            dbBlog.BlogText.DescriptionHead = model.BlogText.DescriptionHead;
            dbBlog.BlogText.Title = model.BlogText.Title;

            await _appDbContext.SaveChangesAsync();

            bool hasError = false;
            int maxSize = 1000;
            if (model.Photo != null)
            {
                if (!_fileService.CheckPhoto(model.Photo))
                {
                    ModelState.AddModelError("Photo", "File must be image");
                    return View(model);
                }
                else if (!_fileService.MaxSize(model.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"{model.Photo.Name} must be less than {maxSize}kb");
                    return View(model);
                }
                _fileService.Delete(_webHostEnvironment.WebRootPath, dbBlog.PhotoPath);
                dbBlog.PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
                await _appDbContext.SaveChangesAsync();
            }

            if (model.BlogPhotos != null)
            {
                foreach (var blogPhoto in model.BlogPhotos)
                {
                    if (!_fileService.CheckPhoto(blogPhoto))
                    {
                        ModelState.AddModelError("BlogPhotos", "Photo must be image");
                        hasError = true;
                    }
                    else if (!_fileService.MaxSize(blogPhoto, maxSize))
                    {
                        ModelState.AddModelError("BlogPhotos", $"{blogPhoto.Name} must be less than {maxSize}kb");
                        hasError = true;
                    }

                    var blogPhotos = new BlogPhoto
                    {
                        BlogId = dbBlog.Id,
                        PhotoPath = await _fileService.UploadAsync(blogPhoto, _webHostEnvironment.WebRootPath)
                    };
                    await _appDbContext.BlogPhotos.AddAsync(blogPhotos);
                    await _appDbContext.SaveChangesAsync();
                }
                if (hasError)
                {
                    return View(model);
                }



            }
            return RedirectToAction("index");
        }


        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dbblog = await _appDbContext.Blogs.Include(bt => bt.BlogText).Include(bp => bp.BlogPhotos).FirstOrDefaultAsync(b => b.Id == id);
            if (dbblog == null) return NotFound();
            var model = new BlogDetailsViewModel
            {
                Id = dbblog.Id,
                Title = dbblog.Title,
                Date = dbblog.Date,
                Description = dbblog.Description,
                Blog = dbblog,
                BlogPhoto = dbblog.BlogPhotos,
                BlogText = dbblog.BlogText

            };
            return View(model);

        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbblog = await _appDbContext.Blogs.FindAsync(id);
            if (dbblog == null) return NotFound();

            _appDbContext.Blogs.Remove(dbblog);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }
        #endregion

        #region DeletePhoto
        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var dbblogphoto = await _appDbContext.BlogPhotos.FindAsync(id);
            if (dbblogphoto == null) return NotFound();

            _fileService.Delete(_webHostEnvironment.WebRootPath, dbblogphoto.PhotoPath);
            _appDbContext.BlogPhotos.Remove(dbblogphoto);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("details", "blog", new {id=dbblogphoto.BlogId});
        }
        #endregion

    }
}

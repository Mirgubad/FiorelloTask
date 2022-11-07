using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.ViewModels;
using FiorelloTaskFronToBack.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloTaskFronToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = new HomeIndexViewModel
            {
                FlowerExperts = await _appDbContext.FlowerExperts.ToListAsync(),
                HomeMainSlider = await _appDbContext.HomeMainSlider
                .Include(hmp => hmp.HomeMainSliderPhotos).FirstOrDefaultAsync()
            };
            return View(model);
        }
    }
}

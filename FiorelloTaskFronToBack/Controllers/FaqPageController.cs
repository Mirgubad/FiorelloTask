using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.ViewModels.FaqPage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloTaskFronToBack.Controllers
{
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
                faqComponents = await _appDbContext.FaqComponents.OrderBy(fc=>fc.Order).ToListAsync()
            };
            return View(model);
        }
    }
}

using FiorelloTaskFronToBack.DAL;
using Microsoft.AspNetCore.Mvc;

namespace FiorelloTaskFronToBack.Areas.Admin.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public TestimonialController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

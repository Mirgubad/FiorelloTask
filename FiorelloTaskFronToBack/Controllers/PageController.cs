using Microsoft.AspNetCore.Mvc;

namespace FiorelloTaskFronToBack.Controllers
{
    public class PageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

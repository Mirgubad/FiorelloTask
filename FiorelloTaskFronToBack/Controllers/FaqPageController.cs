using Microsoft.AspNetCore.Mvc;

namespace FiorelloTaskFronToBack.Controllers
{
    public class FaqPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

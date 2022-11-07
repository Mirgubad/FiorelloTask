using Microsoft.AspNetCore.Mvc;

namespace FiorelloTaskFronToBack.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

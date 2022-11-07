using FiorelloTaskFronToBack.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloTaskFronToBack.ViewComponents
{
    public class TestimonialViewComponent:ViewComponent
    {
        private readonly AppDbContext _appDbContext;

        public TestimonialViewComponent(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
           

            return View();

        }
    }
}

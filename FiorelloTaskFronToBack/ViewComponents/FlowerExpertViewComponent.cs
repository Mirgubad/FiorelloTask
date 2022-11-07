using FiorelloTaskFronToBack.DAL;
using FiorelloTaskFronToBack.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloTaskFronToBack.ViewComponents
{
    public class FlowerExpertViewComponent : ViewComponent
    {
        private readonly AppDbContext _appDbContext;

        public FlowerExpertViewComponent(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var FlowerExperts = await _appDbContext.FlowerExperts.ToListAsync();
            
            return View(FlowerExperts);
        }
    }
}

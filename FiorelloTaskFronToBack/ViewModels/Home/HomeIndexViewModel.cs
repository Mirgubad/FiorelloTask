using FiorelloTaskFronToBack.Models;

namespace FiorelloTaskFronToBack.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public List<Models.FlowerExpert> FlowerExperts { get; set; }
        public HomeMainSlider HomeMainSlider { get; set; }

        public List<Models.Product> Products { get; set; }

        public List<Testimonial> Testimonials { get; set; }
        public List<Models.Blog> Blog { get; set; }

    }
}

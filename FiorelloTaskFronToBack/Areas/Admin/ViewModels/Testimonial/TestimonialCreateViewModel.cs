using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FiorelloTaskFronToBack.Areas.Admin.ViewModels.Testimonial
{
    public class TestimonialCreateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Order { get; set; }
        [Display (Name ="Expert")]
        public int FlowerExpertId { get; set; }
        public List<SelectListItem>? FlowerExpert { get; set; }
    }
}

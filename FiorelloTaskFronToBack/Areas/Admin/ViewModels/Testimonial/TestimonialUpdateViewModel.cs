using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FiorelloTaskFronToBack.Areas.Admin.ViewModels.Testimonial
{
    public class TestimonialUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Order { get; set; }
        [Display(Name = "Expert")]
        public int FlowerExpertId { get; set; }
        public List<SelectListItem>? FlowerExpert { get; set; }


    }
}

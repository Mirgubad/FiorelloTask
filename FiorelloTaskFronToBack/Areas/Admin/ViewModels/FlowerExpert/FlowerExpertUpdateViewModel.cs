using System.ComponentModel.DataAnnotations;

namespace FiorelloTaskFronToBack.Areas.Admin.ViewModels.FlowerExpert
{
    public class FlowerExpertUpdateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Position { get; set; }
        public IFormFile? Photo { get; set; }
    }
}

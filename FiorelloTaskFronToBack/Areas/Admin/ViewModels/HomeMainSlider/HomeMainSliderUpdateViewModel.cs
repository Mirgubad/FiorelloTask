using FiorelloTaskFronToBack.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorelloTaskFronToBack.Areas.Admin.ViewModels.HomeMainSlider
{
    public class HomeMainSliderUpdateViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [NotMapped]
        public IFormFile? SubPhoto { get; set; }
        [NotMapped]
        public List<IFormFile>? SliderPhotos { get; set; }
        public ICollection<HomeMainSliderPhoto>? HomeMainSliderPhotos { get; set; }

    }
}

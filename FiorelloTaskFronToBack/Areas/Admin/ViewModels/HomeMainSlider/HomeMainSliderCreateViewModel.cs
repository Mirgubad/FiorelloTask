using FiorelloTaskFronToBack.Models;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorelloTaskFronToBack.Areas.Admin.ViewModels.HomeMainSlider
{
    public class HomeMainSliderCreateViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [NotMapped]
        public IFormFile SubPhoto { get; set; }
        [NotMapped]
        public List<IFormFile> SliderPhotos { get; set; }
        public ICollection<HomeMainSliderPhoto>? HomeMainSliderPhotos { get; set; }
    }
}

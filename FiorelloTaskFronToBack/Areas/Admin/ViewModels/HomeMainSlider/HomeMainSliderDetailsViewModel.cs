using FiorelloTaskFronToBack.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorelloTaskFronToBack.Areas.Admin.ViewModels.HomeMainSlider
{
    public class HomeMainSliderDetailsViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string SubPhotoPath { get; set; }
        public ICollection<HomeMainSliderPhoto> HomeMainSliderPhotos { get; set; }
    }
}

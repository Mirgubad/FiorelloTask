using FiorelloTaskFronToBack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorelloTaskFronToBack.Areas.Admin.ViewModels.Product
{
    public class ProductCreateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }
        public string Weight { get; set; }
        [Display(Name ="Category")]
        public int CategoryId { get; set; }
        public IFormFile MainPhoto { get; set; }
        public CategoryStatus Status { get; set; }
        public List<ProductPhoto>? ProductPhotos { get; set; }
        public List <SelectListItem>? Categories { get; set; }
        [NotMapped]
        public List<IFormFile> Photos { get; set; }     

    }
}

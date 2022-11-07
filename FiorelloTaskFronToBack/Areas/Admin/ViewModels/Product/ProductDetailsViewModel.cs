using FiorelloTaskFronToBack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorelloTaskFronToBack.Areas.Admin.ViewModels.Product
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }
        public string Weight { get; set; }
        public int CategoryId { get; set; }
        public string PhotoPath { get; set; }
        public CategoryStatus Status { get; set; }
       
        public List<SelectListItem>? Categories { get; set; }

        public List<ProductPhoto> ProductPhotos { get; set; }


    }
}

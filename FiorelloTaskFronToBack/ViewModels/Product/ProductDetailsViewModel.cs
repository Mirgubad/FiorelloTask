using FiorelloTaskFronToBack.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorelloTaskFronToBack.ViewModels.Product
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? MainPhotoPath { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }
        public string Weight { get; set; }
        public int CategoryId { get; set; }

        public CategoryStatus Status { get; set; }
        public List<ProductPhoto> ProductPhotos { get; set; }
        public Category Category { get; set; }

    }
}

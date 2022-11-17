using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorelloTaskFronToBack.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? MainPhotoPath { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public string Weight { get; set; }
        public CategoryStatus Status { get; set; }
  
        
        public List<ProductPhoto> ProductPhotos { get; set; }

        [Display (Name ="Category")]
        public Category Category { get; set; }

        public ICollection<BasketProduct> BasketProducts { get; set; }
    }

    public enum CategoryStatus
    {
        Sold,
        New,
        Sale
    }
}


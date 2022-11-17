using FiorelloTaskFronToBack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace FiorelloTaskFronToBack.Areas.Admin.ViewModels.Product
{
    public class ProductIndexViewModel
    {
        public List<Models.Product> Product { get; set; }


        #region Filter

        public string? Title { get; set; }

        [Display(Name = "Min.Quantity")]
        public int? MinQuantity { get; set; }
        [Display(Name = "Max.Quantity")]
        public int? MaxQuantity { get; set; }

        [Display(Name = "Min.Price")]
        public double? MinPrice { get; set; }
        [Display(Name = "Max.Price")]
        public double? MaxPrice { get; set; }

        public CategoryStatus? Status { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; }




        #endregion
    }
}

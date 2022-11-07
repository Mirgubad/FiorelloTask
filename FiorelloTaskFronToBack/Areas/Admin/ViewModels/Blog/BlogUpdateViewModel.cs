using FiorelloTaskFronToBack.Models;

namespace FiorelloTaskFronToBack.Areas.Admin.ViewModels.Blog
{
    public class BlogUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Photo { get; set; }
        public DateTime? Date { get; set; }
        public BlogPhoto? BlogPhoto { get; set; }
        public List<IFormFile>? BlogPhotos { get; set; }
        public BlogText? BlogText { get; set; }
    }
}

using FiorelloTaskFronToBack.Models;

namespace FiorelloTaskFronToBack.Areas.Admin.ViewModels.Blog
{
    public class BlogDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
       
        public DateTime Date { get; set; }
        public Models.Blog Blog { get; set; }
        public List<BlogPhoto>? BlogPhoto { get; set; }
       
        public BlogText? BlogText { get; set; }
    }
}

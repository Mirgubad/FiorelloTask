using FiorelloTaskFronToBack.Models;
using System.Reflection.Metadata.Ecma335;

namespace FiorelloTaskFronToBack.ViewModels.Blog
{
    public class BlogDetailsViewModel
    {
        public List<BlogPhoto>? BlogPhotos { get; set; }
        public BlogText? Blogtext { get; set; }
        public List<Models.Blog> Blog { get; set; }
        public string MainPhotoPath { get; set; }

    }
}

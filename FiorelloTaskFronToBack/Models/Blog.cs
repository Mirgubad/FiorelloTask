namespace FiorelloTaskFronToBack.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? PhotoPath { get; set; }
        public DateTime Date { get; set; }
        public List<BlogPhoto> BlogPhotos { get; set; }
        public BlogText BlogText { get; set; }
    }
}

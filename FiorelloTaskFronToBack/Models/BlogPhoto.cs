namespace FiorelloTaskFronToBack.Models
{
    public class BlogPhoto
    {
        public int Id { get; set; }
        public string? PhotoPath { get; set; }
        public int  BlogId { get; set; }
        public Blog? Blog { get; set; }
    }
}

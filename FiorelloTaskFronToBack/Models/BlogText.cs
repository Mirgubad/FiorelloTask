namespace FiorelloTaskFronToBack.Models
{
    public class BlogText
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DescriptionHead { get; set; }
        public string DescriptionEnd { get; set; }
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
    }
}

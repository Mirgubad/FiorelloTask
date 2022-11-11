namespace FiorelloTaskFronToBack.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public int FlowerExpertId { get; set; }
        public Models.FlowerExpert FlowerExpert { get; set; }


    }
}

namespace FiorelloTaskFronToBack.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ExpertId { get; set; }
        public Models.FlowerExpert FlowerExperts { get; set; }


    }
}

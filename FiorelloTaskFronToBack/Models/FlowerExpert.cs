using Microsoft.Build.Framework;

namespace FiorelloTaskFronToBack.Models
{
    public class FlowerExpert
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Position { get; set; }
        public string PhotoPath { get; set; }
    }
}

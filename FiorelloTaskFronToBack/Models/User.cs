using Microsoft.AspNetCore.Identity;

namespace FiorelloTaskFronToBack.Models
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }

        public Basket Basket { get; set; }

    }
}

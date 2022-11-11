using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace FiorelloTaskFronToBack.ViewModels.Account
{
    public class AccountRegisterViewModel
    {
        [Required, MaxLength(50)]
        public string Fullname { get; set; }

        [Required, MaxLength(50), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, MaxLength(20)]
        public string Username { get; set; }

        [Required, MaxLength(20), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, MaxLength(20), DataType(DataType.Password), Display(Name = "Confirm Password"), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }




    }
}

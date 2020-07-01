using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.Modals
{
    public class RegistrationModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Password is Limited to {2} to {1} Characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password",ErrorMessage ="The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }



    }

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public class TokenResponse
    {
        public string Token { get; set; }
    }
}

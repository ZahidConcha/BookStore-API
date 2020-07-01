using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.DTOs
{
    public class UserDTO
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Password is Limited to {2} to {1} Characters",MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

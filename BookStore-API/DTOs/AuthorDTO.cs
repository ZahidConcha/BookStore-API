
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.DTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string Fistname { get; set; }
        public string Lastname { get; set; }
        public string Bio { get; set; }

        public IList<BookDTO> ListBooks { get; set; }

    }

    public class AuthorCreateDTO
    {
      
        [Required]
        public string Fistname { get; set; }

        [Required]
        public string Lastname { get; set; }
        public string Bio { get; set; }

     

    }

    public class AuthorUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        public string Fistname { get; set; }
        [Required]
        public string Lastname { get; set; }
        public string Bio { get; set; }

      
    }
}

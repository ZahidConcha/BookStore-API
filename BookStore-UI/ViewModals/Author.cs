using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.ViewModals 
{
    public class Author
    {

        public int Id { get; set; }
        [Required]
        [DisplayName("Fist Name")]
        public string Fistname { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string Lastname { get; set; }
        [Required]
        [DisplayName("Biography")]
        [StringLength(250)]
        public string Bio { get; set; }

        public IList<Book> ListBooks { get; set; }
    }

  
}

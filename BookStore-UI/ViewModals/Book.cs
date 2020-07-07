using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.ViewModals
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public int? Year { get; set; }
        [Required]
        public string Isbn { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        [Required]
        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }
    }
}

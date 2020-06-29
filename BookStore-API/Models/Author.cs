using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace BookStore_API.Modals
{
    [Table("Authors")]
    public partial class Author
    {
        public int Id { get; set; }
        public string Fistname { get; set; }
        public string Lastname { get; set; }
        public string Bio { get; set; }

        public  IList<Book> ListBooks { get; set; }

    }
}
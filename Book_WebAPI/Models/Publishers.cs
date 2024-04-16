using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Book_WebAPI.Models
{
    public class Publishers
    {
        [Key]
        public int PublisherID { get; set; }
        public string? Name { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}

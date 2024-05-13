using System.ComponentModel.DataAnnotations;

namespace Book_WebAPI.Models.Domain
{
    public class Publisher
    {
        [Key]
        public int PublisherId { get; set; }
        public string? PublisherName { get; set; }

        public List<Book> Books { get; set; }
    }
}

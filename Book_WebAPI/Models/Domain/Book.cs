using System.ComponentModel.DataAnnotations;

namespace Book_WebAPI.Models.Domain
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsRead { get; set; }
        public int Rate { get; set; }
        public int Genre { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }

        // One-to-many 
        public int PublisherID { get; set; }
        public Publisher? Publisher { get; set; }

        //Many-to-many 
        public List<BookAuthor>? BookAuthors { get; set; }
    }
}

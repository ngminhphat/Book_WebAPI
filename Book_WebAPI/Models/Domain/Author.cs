using System.ComponentModel.DataAnnotations;

namespace Book_WebAPI.Models.Domain
{
    public class Author
    {
        [Key]
        public int AuthorID { get; set; }
        public string? AuthorName { get; set; }

        public List<BookAuthor>? BookAuthors { get; set; }
    }
}

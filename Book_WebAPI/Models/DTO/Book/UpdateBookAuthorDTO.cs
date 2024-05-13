using System.ComponentModel.DataAnnotations;

namespace Book_WebAPI.Models.DTO.Book
{
    public class UpdateBookAuthorDTO
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

        public int PublisherId { get; set; }
        public List<int>? AuthorId { get; set; }
    }
}

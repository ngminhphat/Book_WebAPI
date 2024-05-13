using System.ComponentModel.DataAnnotations;

namespace Book_WebAPI.Models.DTO.Author
{
    public class UpdateAuthorBookDTO
    {
        [Key]
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }

        public List<int>? BookId { get; set; }
    }
}

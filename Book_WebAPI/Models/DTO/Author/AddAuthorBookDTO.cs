namespace Book_WebAPI.Models.DTO.Author
{
    public class AddAuthorBookDTO
    {
        public string? AuthorName { get; set; }

        public List<int>?BookId { get; set; }

    }
}

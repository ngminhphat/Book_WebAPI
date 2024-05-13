using Book_WebAPI.Models.Domain;


namespace Book_WebAPI.Models.DTO



{
    public class AddBookRequestDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? isRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime? DateAdd { get; set; }
        public int Rate { get; set; }
        public Genre? Genre { get; set; }
        public string? Url { get; set; }
        public int? PublisherId { get; set; }
        public List<int>? AuthorId { get; set; }

        // Add the CoverUrl property
        public string? CoverUrl { get; set; }
    }
}

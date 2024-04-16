using Book_WebAPI.Models;
using System.Text.Json.Serialization;

public class Book
{
    public int BookID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsRead { get; set; }
    public DateTime? DateRead { get; set; }
    public int Rate { get; set; }
    public int Genre { get; set; }
    public string? CoverUrl { get; set; }
    public DateTime DateAdded { get; set; }

    public int PublisherID { get; set; }
    [JsonIgnore]

    public Publishers? Publisher { get; set; }

    public ICollection<Book_WebAPIs>? Book_WebAPIs { get; set; }
}

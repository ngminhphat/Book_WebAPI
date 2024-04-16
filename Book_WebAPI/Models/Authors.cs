using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Book_WebAPI.Models 
{
    public class Author
    {
        public int AuthorID { get; set; }
        public string? FullName { get; set; }
        [JsonIgnore]
        public ICollection<Book_WebAPIs>? Book_WebAPIs { get; set; }
    }
}

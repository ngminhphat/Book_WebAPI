using System.Text.Json.Serialization;

namespace Book_WebAPI.Models
{
    public class Book_WebAPIs
    {
        public int ID { get; set; }

        public int BookID { get; set; }
        [JsonIgnore]
        public Book? Book { get; set; }

        public int AuthorID { get; set; }
        [JsonIgnore]
        public Author? Author { get; set; }
    }
}

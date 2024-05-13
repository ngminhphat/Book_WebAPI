using Book_WebAPI.Models.Domain;

namespace Book_WebAPI.Models.Interfaces
{
    public interface IBookAuthorRepository
    {

        Task<List<BookAuthor>> GetBookAuthorsAsync();
        Task<BookAuthor> GetBookAuthorAsync(int id, bool includeBookAuthors = false);
        Task<BookAuthor> AddBookAuthorAsync(BookAuthor bookAuthor);
        Task<BookAuthor> UpdateBookAuthorAsync(BookAuthor bookAuthor);
        Task<(bool, string)> DeleteBookAuthorAsync(BookAuthor bookAuthor);
    }
}

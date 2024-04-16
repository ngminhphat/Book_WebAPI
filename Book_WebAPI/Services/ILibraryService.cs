using Book_WebAPI.Models;


namespace Book_WebAPI.Services
{
    public interface ILibraryService
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookAsync(int id);
        Task<Book> AddBookAsync(Book book);
        Task<Book> UpdateBookAsync(Book book);
        Task<bool> DeleteBookAsync(int id);

        Task<List<Author>> GetAuthorsAsync();
        Task<Author> GetAuthorAsync(int id);
        Task<Author> AddAuthorAsync(Author author);
        Task<Author> UpdateAuthorAsync(Author author);
        Task<bool> DeleteAuthorAsync(int id);

        Task<List<Publishers>> GetPublishersAsync();
        Task<Publishers> GetPublisherAsync(int id);
        Task<Publishers> AddPublisherAsync(Publishers publisher);
        Task<Publishers> UpdatePublisherAsync(Publishers publisher);
        Task<bool> DeletePublisherAsync(int id);
    }

}
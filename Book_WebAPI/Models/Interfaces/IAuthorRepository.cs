using Book_WebAPI.Models.Domain;
using Book_WebAPI.Models.DTO.Author;

namespace Book_WebAPI.Models.Interfaces
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAuthorsAsync(); 
        Task<Author> GetAuthorAsync(int id, bool includeAuthors = false); 
        Task<AddAuthorBookDTO> AddAuthorAsync(AddAuthorBookDTO author); 
        Task<UpdateAuthorBookDTO> UpdateAuthorAsync(UpdateAuthorBookDTO author); 
        Task<(bool, string)> DeleteAuthorAsync(Author author); 
    }
}

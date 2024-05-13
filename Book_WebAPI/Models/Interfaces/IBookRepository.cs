using Book_WebAPI.Models.Domain;
using Book_WebAPI.Models.DTO.Book;

namespace Book_WebAPI.Models.Interfaces
{
    public interface IBookRepository
    {
        Task<List<GetAllBookDTO>> GetBooksAsync(string? filterOn, string? filterQuery,
                                            string? sortBy, bool isAscending,
                                            int pageNumber, int pageSize); Task<GetBookByIdDTO> GetBookAsync(int id); 

        Task<AddBookAuthorDTO> AddBookAsync(AddBookAuthorDTO book); 
        Task<UpdateBookAuthorDTO> UpdateBookAsync(UpdateBookAuthorDTO book); 
        Task<(bool, string)> DeleteBookAsync(GetBookByIdDTO book);
        Task<List<SearchBook>> SearchBookAsync(string? query);
        Task<List<TestBook>> TestBookAsync(string? query, double? from, double? to, string? sortBy, int page = 1);

    }
}

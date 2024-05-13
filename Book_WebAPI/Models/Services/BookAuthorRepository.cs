using Microsoft.EntityFrameworkCore;
using Book_WebAPI.Data;
using Book_WebAPI.Models.Domain;
using Book_WebAPI.Models.Interfaces;

namespace Book_WebAPI.Models.Services
{
    public class BookAuthorRepository: IBookAuthorRepository
    {
        private readonly AppDbContext _db;
        public BookAuthorRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<BookAuthor>> GetBookAuthorsAsync()
        {
            try
            {
                return await _db.BookAuthors.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<BookAuthor> GetBookAuthorAsync(int id, bool includeBookAuthors)
        {
            try
            {
                if (includeBookAuthors)
                {
                    return await _db.BookAuthors.Include(b => b.Book).FirstOrDefaultAsync(i => i.Id == id);
                }

                return await _db.BookAuthors.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<BookAuthor> AddBookAuthorAsync(BookAuthor bookAuthor)
        {
            try
            {
                await _db.BookAuthors.AddAsync(bookAuthor);
                await _db.SaveChangesAsync();
                return await _db.BookAuthors.FindAsync(bookAuthor.Id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<BookAuthor> UpdateBookAuthorAsync(BookAuthor bookAuthor)
        {
            try
            {
                _db.Entry(bookAuthor).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return bookAuthor;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteBookAuthorAsync(BookAuthor bookAuthor)
        {
            try
            {
                var dbBookAuthor = await _db.BookAuthors.FindAsync(bookAuthor.Id);

                if (dbBookAuthor == null)
                {
                    return (false, "BookAuthor could not be found");
                }

                _db.BookAuthors.Remove(bookAuthor);
                await _db.SaveChangesAsync();

                return (true, "BookAuthor got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Book_WebAPI.Data;
using Book_WebAPI.Models.Domain;
using Book_WebAPI.Models.DTO.Author;
using Book_WebAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Book_WebAPI.Models.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _db;

        public AuthorRepository(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<List<Author>> GetAuthorsAsync()
        {
            return await _db.Authors.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Author> GetAuthorAsync(int id, bool includeAuthors)
        {
            var query = _db.Authors.AsQueryable();
            if (includeAuthors)
            {
                query = query.Include(a => a.BookAuthors);
            }

            return await query.FirstOrDefaultAsync(a => a.AuthorID == id).ConfigureAwait(false);
        }

        public async Task<AddAuthorBookDTO> AddAuthorAsync(AddAuthorBookDTO author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            var addAuthor = new Book_WebAPI.Models.Domain.Author // Adjust the namespace as needed
            {
                AuthorName = author.AuthorName
            };

            _db.Authors.Add(addAuthor);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            foreach (var bookId in author.BookId)
            {
                var bookAuthor = new BookAuthor()
                {
                    BookId = bookId,
                    AuthorId = addAuthor.AuthorID
                };
                _db.BookAuthors.Add(bookAuthor);
            }

            await _db.SaveChangesAsync().ConfigureAwait(false);
            return author;
        }


        public async Task<UpdateAuthorBookDTO> UpdateAuthorAsync(UpdateAuthorBookDTO author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            _db.Entry(author).State = EntityState.Modified;
            await _db.SaveChangesAsync().ConfigureAwait(false);

            return author;
        }

        public async Task<(bool, string)> DeleteAuthorAsync(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            try
            {
                var dbAuthor = await _db.Authors.FindAsync(author.AuthorID).ConfigureAwait(false);

                if (dbAuthor == null)
                {
                    return (false, "Author could not be found");
                }

                _db.Authors.Remove(dbAuthor);
                await _db.SaveChangesAsync().ConfigureAwait(false);

                return (true, "Author deleted successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception here
                return (false, $"An error occurred. Error Message: {ex.Message}");
            }
        }
    }
}

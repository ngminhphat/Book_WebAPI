using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Book_WebAPI.Data;
using Book_WebAPI.Models.Domain;
using Book_WebAPI.Models.DTO.Book;
using Book_WebAPI.Models.Interfaces;

namespace Book_WebAPI.Models.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _db;
        public static int PAGE_SIZE { get; set; } = 2;
        public BookRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<GetAllBookDTO>> GetBooksAsync(string? filterOn, string? filterQuery,
                                            string? sortBy, bool isAscending,
                                            int pageNumber, int pageSize)
        {
            var allbook = _db.Books;
            var d = allbook.Select(b => new GetAllBookDTO()
            {
                BookId = b.BookId,
                Title = b.Title,
                Description = b.Description,
                IsRead = true,
                Rate = b.Rate,
                Genre = b.Genre,
                CoverUrl = b.CoverUrl,
                DateAdded = b.DateAdded,
                PublisherName = b.Publisher.PublisherName,
                AuthorName = b.BookAuthors.Select(a => a.Author.AuthorName).ToList()

            }).AsQueryable();
            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false &&
           string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    d = d.Where(x => x.Title.Contains(filterQuery));
                }
            }
            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    d = isAscending ? d.OrderBy(x => x.Title) :
                   d.OrderByDescending(x => x.Title);
                }
            }
            //pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return d.Skip(skipResults).Take(pageSize).ToList();
        }

    public async Task<GetBookByIdDTO> GetBookAsync(int id)
        {
            try
            {
                var book = await _db.Books
                .Where(b => b.BookId == id)
                .Select(b => new GetBookByIdDTO
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Description = b.Description,
                    IsRead = true,
                    Rate = b.Rate,
                    Genre = b.Genre,
                    CoverUrl = b.CoverUrl,
                    DateAdded = b.DateAdded,
                    PublisherName = b.Publisher.PublisherName,
                    AuthorName = b.BookAuthors.Select(ba => ba.Author.AuthorName).ToList()
                })
                .FirstOrDefaultAsync();
                return book;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex}");
                throw;
            }
        }

        public async Task<AddBookAuthorDTO> AddBookAsync(AddBookAuthorDTO book)
        {
            var addBook = new Book
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                Rate = book.Rate,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                DateAdded = book.DateAdded,
                PublisherID = book.PublisherId
            };

            _db.Books.Add(addBook);
            await _db.SaveChangesAsync();
            foreach (var authorId in book.AuthorId)
            {
                var bookauthor = new BookAuthor()
                {
                    
                    AuthorId = authorId,
                    BookId = addBook.BookId
                };
                _db.BookAuthors.Add(bookauthor);
            }
            await _db.SaveChangesAsync();
            return book;
        }

        public async Task<UpdateBookAuthorDTO> UpdateBookAsync(UpdateBookAuthorDTO book)
        {
            var existingBook = await _db.Books.FindAsync(book.BookId);

            if (existingBook == null)
            {
                return null;
            }
            existingBook.Title = book.Title;
            existingBook.Description = book.Description;
            existingBook.IsRead = book.IsRead;
            existingBook.Rate = book.Rate;
            existingBook.Genre = book.Genre;
            existingBook.CoverUrl = book.CoverUrl;
            existingBook.DateAdded = book.DateAdded;
            existingBook.PublisherID = book.PublisherId;
            
            _db.Entry(existingBook).State = EntityState.Modified;
            _db.BookAuthors.RemoveRange(_db.BookAuthors.Where(sc => sc.BookId == book.BookId));
            // Add new course associations
            foreach (var authorId in book.AuthorId)
            {
                var bookauthor = new BookAuthor()
                {
                    AuthorId = authorId,
                    BookId = book.BookId,
                };

                _db.BookAuthors.Add(bookauthor);
            }
            await _db.SaveChangesAsync();
            return book;
        }

        public async Task<(bool, string)> DeleteBookAsync(GetBookByIdDTO book)
        {
            try
            {
                var dbBook = await _db.Books.FindAsync(book.BookId);

                if (dbBook == null)
                {
                    return (false, "Book could not be found.");
                }

                _db.Books.Remove(dbBook);
                await _db.SaveChangesAsync();

                return (true, "Book got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
        public async Task<List<SearchBook>> SearchBookAsync(string? query)
        {
            var search = _db.Books.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                search = search.Where(st => st.Title.Contains(query));
            }
            var result = await search.Select(st => new SearchBook
            {
                Title = st.Title,
                Description = st.Description,
                IsRead = st.IsRead,
                Rate = st.Rate,
                Genre = st.Genre,
                CoverUrl = st.CoverUrl,
                DateAdded = st.DateAdded
            }).ToListAsync();
            return result;
        }
        public async Task<List<TestBook>> TestBookAsync(string? query, double? from, double? to, string? sortBy, int page = 1)
        {
            var search = _db.Books.AsQueryable();
            //search
            if (!string.IsNullOrEmpty(query))
            {
                search = _db.Books.Where(st => st.Title.Contains(query));
            }
            //filtering
            if (from.HasValue)
            {
                search = search.Where(st => st.Rate >= from);
            }
            if (to.HasValue)
            {
                search = search.Where(st => st.Rate <= to);
            }
            //Default sort by StudentName
            search = search.OrderBy(st => st.Title);
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "title_asc":
                        search = search.OrderBy(st => st.Title);
                        break;
                    case "title_desc":
                        search = search.OrderByDescending(st => st.Title);
                        break;
                    case "genre_asc":
                        search = search.OrderBy(st => st.Genre);
                        break;
                    case "genre_desc":
                        search = search.OrderByDescending(st => st.Genre);
                        break;
                }
            }
            //paging
            search = search.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);

            var result = search.Select(st => new TestBook
            {
                Title = st.Title,
                Description = st.Description,
                IsRead = st.IsRead,
                Rate = st.Rate,
                Genre = st.Genre,
                CoverUrl = st.CoverUrl,
                DateAdded = st.DateAdded
            });
            return result.ToList();
        }
    }
}

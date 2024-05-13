using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Book_WebAPI.Models.Domain;
using Book_WebAPI.Models.DTO.Book;
using Book_WebAPI.Models.Interfaces;
using Book_WebAPI.Data;
using System.Text.Json;

namespace Book_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BooksController> _logger;
        public BooksController(AppDbContext dbContext, IBookRepository bookRepository,
       ILogger<BooksController> logger)
        {
            _dbContext = dbContext;
            _bookRepository = bookRepository;
            _logger = logger;
        }
        //get all books
        // GET: /api/Books/get-all-books?filterOn=Name&filterQuery=Track
        [HttpGet("get-all-books")]
        [Authorize(Roles = "Read")]
        public IActionResult GetAll([FromQuery] string? filterOn, [FromQuery] string?
       filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool isAscending,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            _logger.LogInformation("GetAll Book Action method was invoked");
            _logger.LogWarning("This is a warning log");
            _logger.LogError("This is a error log");
            // su dung reposity pattern
            var allBooks = _bookRepository.GetBooksAsync(filterOn, filterQuery, sortBy,
           isAscending, pageNumber, pageSize);
            //debug
            _logger.LogInformation($"Finished GetAllBook request with data{ JsonSerializer.Serialize(allBooks)}");
        return Ok(allBooks);
        }
        [HttpGet("get-book-by-id")]
        [Authorize(Roles = "Write,Read")]
        public async Task<IActionResult> GetBook(int id)
        {
            GetBookByIdDTO book = await _bookRepository.GetBookAsync(id);

            if (book == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No book found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, book);
        }
        [HttpPost("add-book")]
      //  [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Book>> AddBook([FromBody] AddBookAuthorDTO book)
        {
            if (ModelState.IsValid)
            {
                var dbBook = await _bookRepository.AddBookAsync(book);

                if (dbBook == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"{book.Title} could not be added.");
                }
                return Ok(dbBook);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPut("update-book-by-id")]
        [Authorize(Roles = "Write")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookAuthorDTO book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }
            var dbBook = await _bookRepository.UpdateBookAsync(book);

            if (dbBook == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{book.Title} could not be updated");
            }
            return NoContent();
        }
        [HttpDelete("delete-book-by-id")]
        [Authorize(Roles = "Write")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.GetBookAsync(id);
            (bool status, string message) = await _bookRepository.DeleteBookAsync(book);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            return StatusCode(StatusCodes.Status200OK, book);
        }
        [HttpGet("search-book-by-name")]
        public async Task<IActionResult> SearchBook(string? query)
        {
            try
            {
                var result = await _bookRepository.SearchBookAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("filter-sortby-paging-book")]
        public async Task<IActionResult> TestBook(string? query, double? from, double? to, string? sortBy, int page = 1)
        {
            try
            {
                var result = await _bookRepository.TestBookAsync(query, from, to, sortBy, page);

                if (result == null || result.Count == 0)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    }
}

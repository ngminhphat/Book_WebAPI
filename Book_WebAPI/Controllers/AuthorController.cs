using Book_WebAPI.Models;
using Book_WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Book_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public AuthorsController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            var authors = await _libraryService.GetAuthorsAsync();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _libraryService.GetAuthorAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return author;
        }


        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            var newAuthor = await _libraryService.AddAuthorAsync(author);
            return CreatedAtAction(nameof(GetAuthor), new { id = newAuthor.AuthorID }, newAuthor);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.AuthorID)
            {
                return BadRequest();
            }
            await _libraryService.UpdateAuthorAsync(author);
            return NoContent();
        }

        // DELETE: api/authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var result = await _libraryService.DeleteAuthorAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

}
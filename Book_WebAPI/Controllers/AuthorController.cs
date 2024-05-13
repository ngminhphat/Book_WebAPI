using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Book_WebAPI.Models.Domain;
using Book_WebAPI.Models.DTO.Author;
using Book_WebAPI.Models.Interfaces;

namespace Book_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        [HttpGet("get-all-authors")]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _authorRepository.GetAuthorsAsync();

            if (authors == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No authors in database");
            }

            return StatusCode(StatusCodes.Status200OK, authors);
        }
        [HttpGet("get-author-by-id")]
        public async Task<IActionResult> GetAuthor(int id, bool includeAuthors = false)
        {
            Author author = await _authorRepository.GetAuthorAsync(id, includeAuthors);

            if (author == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No author found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, author);
        }
        [HttpPost("add-author")]
        public async Task<ActionResult<AddAuthorBookDTO>> AddAuthor([FromBody] AddAuthorBookDTO author)
        {
            if (ModelState.IsValid)
            {
                var dbAuthor = await _authorRepository.AddAuthorAsync(author);

                if (dbAuthor == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"{author.AuthorName} could not be added.");
                }
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPut("update-authors-by-id")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] UpdateAuthorBookDTO author)
        {
            if (id != author.AuthorId)
            {
                return BadRequest();
            }
            UpdateAuthorBookDTO dbAuthor = await _authorRepository.UpdateAuthorAsync(author);
            if (dbAuthor == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{author.AuthorId} could not be updated");
            }
            return NoContent();
        }
        [HttpDelete("delete-author-by-id")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorRepository.GetAuthorAsync(id, false);
            (bool status, string message) = await _authorRepository.DeleteAuthorAsync(author);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            return StatusCode(StatusCodes.Status200OK, author);
        }
    }
}

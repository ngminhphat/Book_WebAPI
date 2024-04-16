using Book_WebAPI.Models;
using Book_WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Book_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublishersController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public PublishersController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        // GET: api/publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publishers>>> GetPublishers()
        {
            var publishers = await _libraryService.GetPublishersAsync();
            return Ok(publishers);
        }

        // GET: api/publishers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publishers>> GetPublisher(int id)
        {
            var publisher = await _libraryService.GetPublisherAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return publisher;
        }

        // POST: api/publishers
        [HttpPost]
        public async Task<ActionResult<Publishers>> PostPublisher(Publishers publisher)
        {
            var newPublisher = await _libraryService.AddPublisherAsync(publisher);
            return CreatedAtAction(nameof(GetPublisher), new { id = newPublisher.PublisherID }, newPublisher);
        }

        // PUT: api/publishers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, Publishers publisher)
        {
            if (id != publisher.PublisherID)
            {
                return BadRequest();
            }
            await _libraryService.UpdatePublisherAsync(publisher);
            return NoContent();
        }

        // DELETE: api/publishers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var result = await _libraryService.DeletePublisherAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

}
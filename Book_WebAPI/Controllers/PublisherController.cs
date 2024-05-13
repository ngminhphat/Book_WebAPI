using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Book_WebAPI.Models.Domain;
using Book_WebAPI.Models.Interfaces;

namespace Book_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherControllers : ControllerBase
    {
        private readonly IPublisherRepository _publisherRepository;
        public PublisherControllers(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }
        [HttpGet("get-all-publishers")]
        public async Task<IActionResult> GetPublishers()
        {
            var publishers = await _publisherRepository.GetPublishersAsync();

            if (publishers == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No publishers in database");
            }

            return StatusCode(StatusCodes.Status200OK, publishers);
        }
        [HttpGet("get-publishers-by-id")]
        public async Task<IActionResult> GetPublisher(int id, bool includePublishers = false)
        {
            Publisher publishers = await _publisherRepository.GetPublisherAsync(id, includePublishers);

            if (publishers == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No publishers found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, publishers);
        }
        [HttpPost("add-publisher")]
        public async Task<ActionResult<Publisher>> AddPublisher(Publisher publisher)
        {
            var dbPublisher = await _publisherRepository.AddPublisherAsync(publisher);

            if (dbPublisher == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{publisher.PublisherName} could not be added.");
            }

            return CreatedAtAction("GetPublisher", new { id = publisher.PublisherId }, publisher);
        }
        [HttpPut("update-publisher-by-id")]
        public async Task<IActionResult> UpdatePublisher(int id, Publisher publisher)
        {
            if (id != publisher.PublisherId)
            {
                return BadRequest();
            }
            Publisher dbPublisher = await _publisherRepository.UpdatePublisherAsync(publisher);
            if (dbPublisher == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{publisher.PublisherName} could not be updated");
            }
            return NoContent();
        }
        [HttpDelete("delete-publisher-by-id")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publishers = await _publisherRepository.GetPublisherAsync(id, false);
            (bool status, string message) = await _publisherRepository.DeletePublisherAsync(publishers);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            return StatusCode(StatusCodes.Status200OK, publishers);
        }
    }
}

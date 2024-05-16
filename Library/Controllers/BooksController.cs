using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Library.Models.DTO;
using System.Text.Json;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IHttpClientFactory httpClientFactory, ILogger<BooksController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index([FromQuery] string filterOn = null, string filterQuery = null, string sortBy = null, bool isAscending = true)
        {
            try
            {
                List<BookDTO> response = new List<BookDTO>();

                // Lấy dữ liệu books from API
                var client = _httpClientFactory.CreateClient();
                var httpResponse = await client.GetAsync("https://localhost:7001/api/Books/get-all-books?filterOn=" + filterOn + "&filterQuery=" + filterQuery + "&sortBy=" + sortBy + "&isAscending=" + isAscending);

                httpResponse.EnsureSuccessStatusCode();
                response.AddRange(await httpResponse.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>());

                return View(response);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while fetching data from API.");
                return View("Error", ex.Message);
            }
        }


    }




}

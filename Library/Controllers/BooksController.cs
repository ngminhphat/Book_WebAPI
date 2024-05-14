using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Library.Models.DTO;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public BooksController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index([FromQuery] string filteron = null, string filterQuery = null, string sortBy = null, bool isAscending = true)
        {
            List<BookDTO> response = new List<BookDTO>();
            try
            {
                // lấy dữ liệu books from API
                var client = httpClientFactory.CreateClient();
                var httpResponseMess = await client.GetAsync("https://localhost:7001/api/Books/get-all-books?filterOn=" + filteron + "&filterQuery=" + filterQuery + "&sortBy=" + sortBy + "&isAscending=" + isAscending);
                httpResponseMess.EnsureSuccessStatusCode(); 
                response.AddRange(await httpResponseMess.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>());
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            return View(response);
        }
    }

}

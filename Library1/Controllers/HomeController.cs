using Library1.Models;
using Library1.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Library1.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var httpResponseMessage = await _httpClient.GetAsync("https://localhost:7001/api/Books/get-all-books");
                

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var content = await httpResponseMessage.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<List<BookDTO>>(content);
                    List<BookDTO> books = model ?? new List<BookDTO>();
                    return View(books);
                }
                
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            return View("Error");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

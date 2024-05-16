using Microsoft.AspNetCore.Mvc;
using Library1.Models.DTO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace Library1.Controllers
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
            try
            {
                // Encode query parameter values
                filteron = Uri.EscapeDataString(filteron ?? "");
                filterQuery = Uri.EscapeDataString(filterQuery ?? "");
                sortBy = Uri.EscapeDataString(sortBy ?? "");

                // Construct URL
                var apiUrl = $"https://localhost:7001/api/Books/get-all-books?filterOn={filteron}&filterQuery={filterQuery}&sortBy={sortBy}&isAscending={isAscending}";

                // Make API request
                using (var client = httpClientFactory.CreateClient())
                {
                    var httpResponse = await client.GetAsync(apiUrl).ConfigureAwait(false);
                    httpResponse.EnsureSuccessStatusCode();

                    var books = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>().ConfigureAwait(false);

                    return View(books);
                }
            }
            catch (Exception ex)
            {
                

                // Return error view
                return View("Error");
            }
        }

        public IActionResult AddBook()
        {
            ViewBag.ListPublisher = new List<BookDTO>();
            ViewBag.ListAuthor = new List<AddBookDTO>();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookDTO addBookDTO)
{
    try
    {
        var client = httpClientFactory.CreateClient();
        
        // Lấy danh sách tác giả
        var authorResponse = await client.GetAsync("https://localhost:7001/api/Author/get-all-authors");
        authorResponse.EnsureSuccessStatusCode();
        var authors = await authorResponse.Content.ReadFromJsonAsync<IEnumerable<AuthorDTO>>();
        
        // Lấy danh sách nhà xuất bản
        var publisherResponse = await client.GetAsync("https://localhost:7001/api/Publisher/get-all-publishers");
        publisherResponse.EnsureSuccessStatusCode();
        var publishers = await publisherResponse.Content.ReadFromJsonAsync<IEnumerable<PublisherDTO>>();
        
        // Đưa danh sách vào ViewBag
        ViewBag.ListAuthor = authors;
        ViewBag.ListPublisher = publishers;
        
        return View();
    }
    catch (Exception ex)
    {
        // Xử lý lỗi ở đây
        ViewBag.Error = ex.Message;
        return View("Error");
    }
}

        
        public async Task<IActionResult> ListBook(int id)
        {
            BookDTO response = new BookDTO();
            try
            {
                // Lấy dữ liệu books từ API
                var client = httpClientFactory.CreateClient();
                var httpResponseMess = await client.GetAsync("https://localhost:7001/api/Books/get-book-by-id?id=" + id);
                httpResponseMess.EnsureSuccessStatusCode();
                var stringResponseBody = await httpResponseMess.Content.ReadAsStringAsync();
                response = await httpResponseMess.Content.ReadFromJsonAsync<BookDTO>();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> editBook(int id)
        {
            BookDTO responseBook = new BookDTO();
            var client = httpClientFactory.CreateClient();
            var httpResponseMess = await client.GetAsync("https://localhost:7001/api/Books/get-book-by-id?id=" + id);
            httpResponseMess.EnsureSuccessStatusCode();
            responseBook = await httpResponseMess.Content.ReadFromJsonAsync<BookDTO>();
            ViewBag.Book = responseBook;

            List<AuthorDTO> responseAu = new List<AuthorDTO>();
            var httpResponseAu = await client.GetAsync("https://localhost:7001/api/Author/get-all-authors");
            httpResponseAu.EnsureSuccessStatusCode();
            responseAu = (await httpResponseAu.Content.ReadFromJsonAsync<IEnumerable<AuthorDTO>>()).ToList();
            ViewBag.ListAuthor = responseAu;
            List<PublisherDTO> responsePu = new List<PublisherDTO>();
            var httpResponsePu = await client.GetAsync("https://localhost:7001/api/Publisher/get-all-publishers");
         
            httpResponsePu.EnsureSuccessStatusCode();
            responsePu = (await httpResponsePu.Content.ReadFromJsonAsync<IEnumerable<PublisherDTO>>()).ToList();
            ViewBag.ListPublisher = responsePu;
            return View();
        }
        public IActionResult EditBook()
        {
            ViewBag.listPublisher = new List<BookDTO>();
            ViewBag.listAuthor = new List<AddBookDTO>();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditBook(int id, editBookDTO bookDTO)
        {
            try
            {
                // Gửi dữ liệu sách đã chỉnh sửa đến API để cập nhật
                var client = httpClientFactory.CreateClient();
                var httpRequestMess = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri("https://localhost:7001/api/Books/update-book-by-id?id=" + id),
                    Content = new StringContent(JsonConvert.SerializeObject(bookDTO), Encoding.UTF8, "application/json")

                };

                var httpResponseMess = await client.SendAsync(httpRequestMess);
                httpResponseMess.EnsureSuccessStatusCode();
                var response = await httpResponseMess.Content.ReadFromJsonAsync<AddBookDTO>();
                if (response != null)
                {
                    return RedirectToAction("Index", "Books");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            try
            {
                // Lấy dữ liệu books từ API
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.DeleteAsync("https://localhost:7001/api/Books/delete-book-by-id?id=" + id);
                httpResponseMessage.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Books");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View("Index");
        }




    }

}

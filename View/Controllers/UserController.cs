using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace View.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private string apiUrl = "";
        public UserController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiUrl = "https://localhost:7241/api";
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}

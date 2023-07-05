using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using View.Models;

namespace View.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private string apiUrl = "";

        public HomeController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiUrl = "https://localhost:7241/api/auth";
        }

        public IActionResult Index()
        {
            string? authToken = Request.Cookies["AuthToken"];

            if (authToken != null)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", authToken);
                string authorizationHeader = _httpClient.DefaultRequestHeaders.Authorization!.ToString();
                Console.WriteLine("Token in header:"+authorizationHeader);
            }
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
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
            string? role = Request.Cookies["Role"];
            if (role!=null)
            {
                if (role.Equals("Staff") || (role.Equals("Admin"))) return RedirectToAction("Index", "Admin");
                if (role.Equals("User") || (role.Equals("Lecturer"))) return View();
                TempData["Error"] = "An error has occured";
            }
            return View();
        }

        public IActionResult AboutUs()
        {
            var role = Request.Cookies["Role"];
            if (role == "Admin" || role == "Staff")
            {
                return RedirectToAction("Index", "Admin");
            }
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
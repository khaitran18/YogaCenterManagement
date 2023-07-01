using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using View.Models;

namespace View.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var requestHeaders = Request.Headers;
            if (requestHeaders.ContainsKey("Authorization"))
            {
                var headerValue = requestHeaders["Authorization"].ToString();
                Console.WriteLine("Header value of Authorization:"+headerValue);
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
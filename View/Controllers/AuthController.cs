using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace View.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;
        private string apiUrl = "";
        public AuthController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiUrl = "https://localhost:7241/api/auth";
        }
        public IActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginDto loginDto)
        //{
        //    var loginUrl = apiUrl+"/login";
        //    var json = JsonSerializer.Serialize(loginDto);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync(loginUrl, content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var resultJson = await response.Content.ReadAsStringAsync();
        //        var options = new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true,
        //        };
        //        var loginResult = JsonSerializer.Deserialize<UserDto>(resultJson, options);
        //        if (loginResult?.IsAdmin == true)
        //        {
        //            HttpContext.Session.SetString("Status", "LoggedIn");
        //            HttpContext.Session.SetString("IsAdmin", "True");
        //            return RedirectToAction("Index", "Employee");
        //        }
        //        else
        //        {
        //            HttpContext.Session.SetString("Status", "LoggedIn");
        //            HttpContext.Session.SetString("IsAdmin", "False");
        //            HttpContext.Session.SetInt32("EmployeeId", loginResult?.EmployeeID ?? 0);
        //            return RedirectToAction("EmployeeIndex", "ParticipatingProject");
        //        }
        //    }
        //    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        //    {
        //        ViewData["Message"] = "Wrong email or password";
        //    }
        //    return View(loginDto);
        //}
    }
}

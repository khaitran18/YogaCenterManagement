using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using View.Models;
using View.Models.Response;

namespace View.Controllers
{
    [Route("")]
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

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var loginUrl = apiUrl + "/login";
            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(loginUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var loginResult = JsonSerializer.Deserialize<BaseResponse<AuthResponseDto>>(resultJson, options);

                // Set token to header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult?.Result.Token);

                // Check authority
                TempData["Success"] = "Login success";
                if (loginResult.Result.Role.Equals("User"))
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (loginResult.Result.Role.Equals("Lecturer"))
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (loginResult.Result.Role.Equals("Staff"))
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (loginResult.Result.Role.Equals("Admin"))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Error"] = "Wrong username or password";
            }
            return View(loginDto);
        }

        [HttpGet("signup")]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost("signup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(SignupDto signupDto)
        {
            var signup = apiUrl + "/signup";
            var json = JsonSerializer.Serialize(signupDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(signup, content);
            var resultJson = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var result = JsonSerializer.Deserialize<BaseResponse<UserDto>>(resultJson, options);
            if (result.Error)
            {
                return View(result.Message);
            }
            else
            {
                TempData["Success"] = "Check email to verify your account";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}

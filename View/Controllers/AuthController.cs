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
            var resultJson = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (response.IsSuccessStatusCode)
            {
                var loginResult = JsonSerializer.Deserialize<BaseResponse<AuthResponseDto>>(resultJson, options);

                // Set token to header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult?.Result.Token);

                // Set token as a cookie
                var cookieOptions = new CookieOptions
                {
                    // Set other options as needed (e.g., expiration, domain, path, secure, etc.)
                    Expires = DateTimeOffset.Now.AddDays(1),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };

                // Set the cookie
                Response.Cookies.Append("AuthToken", loginResult?.Result.Token, cookieOptions);
                Response.Cookies.Append("Role", loginResult.Result.Role, cookieOptions);
                Response.Cookies.Append("Id", loginResult.Result.UserId.ToString(), cookieOptions);

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
                    return RedirectToAction("Index", "Admin");
                }
                else if (loginResult.Result.Role.Equals("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                var error = JsonSerializer.Deserialize<string>(resultJson, options);
                TempData["Error"] = string.Join("\n", error);
                return View();
            }
        }

        [HttpGet("verify")]
        public IActionResult Verify([FromQuery] string t)
        {
            ViewBag.Token = t;
            return View("Verify");
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
            var signup = apiUrl + "/signup/";
            var json = JsonSerializer.Serialize(signupDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(signup, content);
            var resultJson = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<BaseResponse<UserDto>>(resultJson, options);
                if (result.Error)
                {
                    TempData["Error"] = await response.Content.ReadAsStringAsync();
                    return View(result.Message);
                }
                else
                {
                    TempData["Success"] = "Check email to verify your account";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                var error = JsonSerializer.Deserialize<string>(resultJson, options);
                TempData["Error"] = string.Join("\n", error);
                return View();
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("Role");
            Response.Cookies.Delete("Id");
            TempData["Success"] = "Logout success";
            return RedirectToAction("Index", "Home");
        }
    }
}

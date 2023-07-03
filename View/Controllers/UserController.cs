using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using View.Models.Response;

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
            apiUrl = "https://localhost:7241/api/user";
        }

        private string? GetAuthTokenFromCookie()
        {
            var authToken = Request.Cookies["AuthToken"];
            return authToken;
        }

        private void AddAuthTokenToRequestHeaders()
        {
            var authToken = GetAuthTokenFromCookie();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        public IActionResult Details()
        {
            var user = new UserDto
            {
                FullName = "Nguyễn Thọ Nguyên",
                Email = "gasshu49@gmail.com",
                Address = "HCM",
                Phone = "0933424515",
            };
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDto user)
        {
            AddAuthTokenToRequestHeaders();
            var url = apiUrl + "/profile";
            var jsonCommand = JsonSerializer.Serialize(user);
            var content = new StringContent(jsonCommand, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<UserDto>>(responseBody, options);

                if (!baseResponse!.Error)
                {
                    return RedirectToAction("Details");
                }
                else
                {
                    ViewBag.ErrorMessage = baseResponse.Message;
                    return View(user);
                }
            }
            else
            {
                return View(user);
            }
        }
    }
}

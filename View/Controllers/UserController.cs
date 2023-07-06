using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using View.Models;
using View.Models.Response;

namespace View.Controllers
{
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

        public async Task<IActionResult> Details()
        {
            string url = apiUrl + "/profile";
            AddAuthTokenToRequestHeaders();
            var response = await _httpClient.GetAsync(url);
            var resultJson = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (response.IsSuccessStatusCode)
            {
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<UserDto>>(resultJson, options);
                return View(baseResponse.Result);
            }
            else
            {
                TempData["Error"] = "Error";
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDto user)
        {
            AddAuthTokenToRequestHeaders();
            var url = apiUrl + "/profile";
            var jsonCommand = JsonSerializer.Serialize(user);
            var content = new StringContent(jsonCommand, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (response.IsSuccessStatusCode)
            {    
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<UserDto>>(responseBody, options);

                if (!baseResponse!.Error)
                {
                    TempData["Success"] = "Edit success";
                    return RedirectToAction("Details");
                }
                else
                {
                    TempData["Error"] = baseResponse.Message;
                    return RedirectToAction("Details");
                }
            }
            else
            {
                var error = JsonSerializer.Deserialize<string>(responseBody, options);
                TempData["Error"] = string.Join("\n", error);
                return RedirectToAction("Details");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeedback(string subject, int lecturerId, int classId)
        {
            AddAuthTokenToRequestHeaders();

            var command = new FeedbackDto
            {
                Content = subject,
                LecturerId = lecturerId
            };

            var jsonCommand = JsonSerializer.Serialize(command);
            var content = new StringContent(jsonCommand, Encoding.UTF8, "application/json");

            var url = apiUrl + "/feedback";
            var response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (response.IsSuccessStatusCode)
            {
                
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<FeedbackDto>>(responseBody, options);

                if (!baseResponse!.Error)
                {
                    TempData["Success"] = "Send feedback success";
                    return RedirectToAction("StudyingClassesById", "Class", new { classId = classId });
                }
                else
                {
                    TempData["Error"] = baseResponse.Message;
                    return RedirectToAction("StudyingClassesById", "Class", new { classId = classId });
                }
            }
            else
            {
                var error = JsonSerializer.Deserialize<string>(responseBody, options);
                TempData["Error"] = string.Join("\n", error);
                return RedirectToAction("StudyingClassesById", "Class", new { classId = classId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Feedback(int page = 1, int pageSize = 5)
        {
            AddAuthTokenToRequestHeaders();
            var url = apiUrl + $"/feedback?page={page}&pageSize={pageSize}";
            var response = await _httpClient.GetAsync(url);
            var role = Request.Cookies["Role"];

            if (response.IsSuccessStatusCode && role == "Lecturer")
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<PaginatedResult<FeedbackDto>>>(responseBody, options);

                if (!baseResponse!.Error)
                {
                    return View(baseResponse.Result);
                }
                else
                {
                    ViewBag.ErrorMessage = baseResponse.Message;
                    return View();
                }
            }
            else if (role == "Admin" || role == "Staff")
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;
using View.Models;
using View.Models.Response;

namespace View.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private string apiUrl = "";
        private string studySlotApiUrl = "";
        private string changeClassRequestsApiUrl = "";
        public AdminController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiUrl = "https://localhost:7241/api/user";
            studySlotApiUrl = "https://localhost:7241/api/Class/slot";
            changeClassRequestsApiUrl = "https://localhost:7241/api/Class/changeclass";
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

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Users(string? searchKeyword, int? roleId, bool? isDisabled, bool? isVerified, string? sortBy, int? page, int? pageSize)
        {
            AddAuthTokenToRequestHeaders();
            var queryString = BuildQueryString(searchKeyword, roleId, isDisabled, isVerified, sortBy, page, pageSize);
            var queryStringWithoutPage = RemoveQueryStringParameter(queryString, "page");
            var url = apiUrl + queryString;
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<PaginatedResult<UserDto>>>(responseBody, options);

                if (!baseResponse!.Error)
                {
                    ViewBag.QueryString = queryStringWithoutPage;
                    return View(baseResponse.Result);
                }
                else
                {
                    ViewBag.ErrorMessage = baseResponse.Message;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> StudySlots()
        {
            var response = await _httpClient.GetAsync(studySlotApiUrl);
            
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();


                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<IEnumerable<StudySlotDto>>>(responseBody, options);
                //Console.WriteLine(baseResponse);
                if (!baseResponse!.Error)
                {

                    return View(baseResponse.Result);
                }
                else
                {
                    var errorResponse = new BaseResponse<Exception>
                    {
                        Error = true,
                        Message = baseResponse.Message,
                        Exception = baseResponse.Exception
                    };

                    ViewBag.ErrorResponse = errorResponse;
                    return View();
                }
               
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> ChangeClassRequests()
        {
            var response = await _httpClient.GetAsync(changeClassRequestsApiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<IEnumerable<ChangeClassRequestDto>>>(responseBody, options);
                Console.WriteLine(baseResponse);
                if (!baseResponse!.Error)
                {

                    return View(baseResponse.Result);
                }
                else
                {
                    var errorResponse = new BaseResponse<Exception>
                    {
                        Error = true,
                        Message = baseResponse.Message,
                        Exception = baseResponse.Exception
                    };

                    ViewBag.ErrorResponse = errorResponse;
                    return View();
                }

            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> StudySlots(TimeSpan start, TimeSpan end, List<int> days)
        {
            var requestData = new
            {
                token = "",
                startTime = start.ToString(),
                endTime = end.ToString(),
                dateIds = days
            };
            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJraGFpdHJhbnF1YW5nIiwianRpIjoiMTgiLCJ1c2VybmFtZSI6ImtoYWl0cmFucXVhbmciLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiZXhwIjoxNjkwOTAyMzY0LCJpc3MiOiJqd3QiLCJhdWQiOiJqd3QifQ.-zZevseiqLHOfIR1pyrlg8mF5tTRx74w6-9aFZ3tyco");

            var response = await _httpClient.PostAsync(studySlotApiUrl, content);
            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (response.IsSuccessStatusCode)
            {
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<StudySlotDto>>(responseBody, options);
                TempData["Success"] = "Create successfully";
            }
            else
            {
                var error = JsonSerializer.Deserialize<string>(responseBody, options);
                Console.WriteLine(error);
                TempData["Error"] = string.Join("\n", error);
                return RedirectToAction(nameof(StudySlots));
            }
            //Console.WriteLine(baseResponse);
            
            return RedirectToAction(nameof(StudySlots));
        }

        [HttpPost]
        public async Task<IActionResult> StudySlotsDelete([FromForm] int slotId)
        {
            Console.WriteLine(slotId);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJraGFpdHJhbnF1YW5nIiwianRpIjoiMTgiLCJ1c2VybmFtZSI6ImtoYWl0cmFucXVhbmciLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiZXhwIjoxNjkwOTAyMzY0LCJpc3MiOiJqd3QiLCJhdWQiOiJqd3QifQ.-zZevseiqLHOfIR1pyrlg8mF5tTRx74w6-9aFZ3tyco");
            var response = await _httpClient.DeleteAsync($"{studySlotApiUrl}/{slotId}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (response.IsSuccessStatusCode)
            {
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<bool>>(responseBody, options);
                TempData["Success"] = "Delete successfully";
            }
            else
            {
                var error = JsonSerializer.Deserialize<string>(responseBody, options);
                Console.WriteLine(error);
                TempData["Error"] = string.Join("\n", error);
                return RedirectToAction(nameof(StudySlots));
            }
            //Console.WriteLine(baseResponse);

            return RedirectToAction(nameof(StudySlots));
        }
        //private string BuildQueryString(int? roleId, bool? disabled, bool? verified, string sortBy, int? page, int? pageSize)

        [HttpPost]
        public async Task<IActionResult> DisableUser(int userId)

        {
            AddAuthTokenToRequestHeaders();
            var disableUserDto = new DisableUserDto
            {
                Reason = "You have been banned!"
            };

            var url = apiUrl + $"/disable/{userId}";

            var jsonCommand = JsonSerializer.Serialize(disableUserDto);
            var content = new StringContent(jsonCommand, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<UserDto>>(responseBody, options);

                if (!baseResponse!.Error)
                {
                    return RedirectToAction("Users");
                }
                else
                {
                    ViewBag.ErrorMessage = baseResponse.Message;
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "An error occurred while disabling the user.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserDto user)
        {
            AddAuthTokenToRequestHeaders();

            var jsonCommand = JsonSerializer.Serialize(user);
            var content = new StringContent(jsonCommand, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<UserDto>>(responseBody, options);

                if (!baseResponse!.Error)
                {
                    return RedirectToAction("Users");
                }
                else
                {
                    ViewBag.ErrorMessage = baseResponse.Message;
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "An error occurred while updating the user.";
                return View();
            }
        }
        private string BuildQueryString(string? searchKeyword, int? roleId, bool? disabled, bool? verified, string? sortBy, int? page, int? pageSize)
        {
            var queryString = "";

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                queryString += $"searchKeyword={searchKeyword}&";
            }

            if (roleId != null)
            {
                queryString += $"roleId={roleId}&";
            }

            if (disabled != null)
            {
                queryString += $"isDisabled={disabled}&";
            }

            if (verified != null)
            {
                queryString += $"isVerified={verified}&";
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                queryString += $"sortBy={sortBy}&";
            }

            if (page != null)
            {
                queryString += $"page={page}&";
            }

            if (pageSize != null)
            {
                queryString += $"pageSize={pageSize}&";
            }

            if (queryString.EndsWith("&"))
            {
                queryString = queryString.TrimEnd('&');
            }

            if (queryString != "")
            {
                queryString = "?" + queryString;
            }

            return queryString;
        }

        private string RemoveQueryStringParameter(string queryString, string key)
        {
            var collection = HttpUtility.ParseQueryString(queryString);
            collection.Remove(key);
            return "?" + collection.ToString();
        }
    }
}

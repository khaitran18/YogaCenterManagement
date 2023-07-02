using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
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
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Users(int? roleId, bool? disabled, bool? verified, string sortBy, int? page, int? pageSize)
        {
            var queryString = BuildQueryString(roleId, disabled, verified, sortBy, page, pageSize);
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
                    baseResponse.Result.QueryString = queryString;
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
        private string BuildQueryString(int? roleId, bool? disabled, bool? verified, string sortBy, int? page, int? pageSize)
        {
            var queryString = "?";

            if (roleId != null)
            {
                queryString += $"roleId={roleId}&";
            }

            if (disabled != null)
            {
                queryString += $"disabled={disabled}&";
            }

            if (verified != null)
            {
                queryString += $"verified={verified}&";
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

            return queryString.TrimEnd('&');
        }
    }
}

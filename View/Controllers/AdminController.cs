﻿using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;
using View.Models;
using View.Models.Enum;
using View.Models.Response;

namespace View.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private string apiUrl = "";
        private string classApiUrl = "";
        private string studySlotApiUrl = "";
        private string changeClassRequestsApiUrl = "";
        public AdminController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiUrl = "https://localhost:7241/api/user";
            classApiUrl = "https://localhost:7241/api/class";
            studySlotApiUrl = "https://localhost:7241/api/Class/slot";
            changeClassRequestsApiUrl = "https://localhost:7241/api/Class/changeclass";
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users(string? searchKeyword, int? roleId, bool? isDisabled, bool? isVerified, string? sortBy, int? page, int? pageSize)
        {
            AddAuthTokenToRequestHeaders();
            var queryString = BuildQueryStringUsers(searchKeyword, roleId, isDisabled, isVerified, sortBy, page, pageSize);
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

        public async Task<IActionResult> Classes(string? searchKeyword, string? sortBy, DateTime? startingFromDate, int? durationMonths, string? classCapacity, int? page, int? pageSize)
        {
            AddAuthTokenToRequestHeaders();
            var queryString = BuildQueryStringClasses(searchKeyword, sortBy, startingFromDate, durationMonths, classCapacity, page, pageSize);
            var queryStringWithoutPage = RemoveQueryStringParameter(queryString, "page");
            var url = classApiUrl + queryString;
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<PaginatedResult<ClassDto>>>(responseBody, options);

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

        //public async Task<IActionResult> ClassDetails(int classId)
        //{

        //}

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
        public async Task<IActionResult> UpdateSlot(int slotId,TimeSpan start, TimeSpan end, List<int> days)
        {
            List<DayDto> dayDtos = days.Select(dayId => new DayDto
            {
                DayId = dayId,
                Day = ((Days)dayId).ToString()
            }).ToList();

            var requestData = new
            {
                studySlot = new
                {
                    slotId = slotId,
                    startTime = start.ToString(),
                    endTime = end.ToString(),
                    day = dayDtos
                }
            };

            var jsonCommand = JsonSerializer.Serialize(requestData);
            Console.WriteLine(jsonCommand);
            var content = new StringContent(jsonCommand, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(studySlotApiUrl, content);
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<bool>>(responseBody, options);

                if (!baseResponse!.Error)
                {
                    TempData["Success"] = "Update Successfully";
                    return RedirectToAction(nameof(StudySlots));
                }
                else
                {
                    TempData["Error"] = baseResponse.Message;
                    return RedirectToAction(nameof(StudySlots));
                }
            }
            else
            {
                TempData["Error"] = "Error update study slot";
                return RedirectToAction(nameof(StudySlots));
            }
            //return RedirectToAction(nameof(StudySlots));
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

        [HttpPost]
        public async Task<IActionResult> DisableUser(int userId, string reason)

        {
            AddAuthTokenToRequestHeaders();
            var disableUserDto = new DisableUserDto
            {
                Reason = reason
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

        public async Task<IActionResult> UpdateApprovalStatus(int requestId, short isApproved)
        {
            var requestData = new
            {
                RequestId = requestId,
                IsApproved = isApproved
            };

            var jsonCommand = JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonCommand, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(changeClassRequestsApiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<bool>>(responseBody, options);

                if (!baseResponse!.Error)
                {
                    TempData["Success"] = isApproved == 0 ? "Deny successfully" : "Approve successfully";
                    return RedirectToAction(nameof(ChangeClassRequests));
                }
                else
                {
                    TempData["Error"] = baseResponse.Message;
                    return RedirectToAction(nameof(ChangeClassRequests));
                }
            }
            else
            {
                TempData["Error"] = "Error occur while approve/deny request";
                return RedirectToAction(nameof(ChangeClassRequests));
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

        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(SignupDto signupDto)
        {
            var signup = "https://localhost:7241/api/auth/signup/"+signupDto.Role;

            // Add auth token to request header
            string authToken = Request.Cookies["AuthToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            // Call to the api
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
                TempData["Success"] = "Create account successful";
                return View();
            }
            else
            {
                TempData["Error"] = await response.Content.ReadAsStringAsync();
                return View();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Classes([FromForm]CreateClassDto model )
        {
            //Add auth token to request header
            string authToken = Request.Cookies["AuthToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var formData = new MultipartFormDataContent();
            Console.WriteLine(model.Monday);
            var file = model.Image;
            if(file != null && file.Length > 0)
            {
                byte[] data;
                using (var br = new BinaryReader(file.OpenReadStream()))
                    data = br.ReadBytes((int)file.OpenReadStream().Length);

                ByteArrayContent bytes = new ByteArrayContent(data);
                formData.Add(bytes, "Image", file.FileName);
            }
            else
            {
                Console.WriteLine("File is empty");
            }
            formData.Add(new StringContent(model.ClassName), "ClassName");
            formData.Add(new StringContent(model.ClassCapacity.ToString()), "ClassCapacity");
            formData.Add(new StringContent(model.Price.ToString()), "Price");
            formData.Add(new StringContent(model.Description), "Description");
            formData.Add(new StringContent(model.StartDate.ToString("yyyy-MM-dd")), "StartDate");
            formData.Add(new StringContent(model.EndDate.ToString("yyyy-MM-dd")), "EndDate");
            formData.Add(new StringContent(SelectDay(model)), "SelectedDayOfWeek");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJraGFpdHJhbnF1YW5nIiwianRpIjoiMTgiLCJ1c2VybmFtZSI6ImtoYWl0cmFucXVhbmciLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiZXhwIjoxNjkwOTAyMzY0LCJpc3MiOiJqd3QiLCJhdWQiOiJqd3QifQ.-zZevseiqLHOfIR1pyrlg8mF5tTRx74w6-9aFZ3tyco");

            var response = await _httpClient.PostAsync(classApiUrl, formData);
            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (response.IsSuccessStatusCode)
            {
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<ClassDto>>(responseBody, options);
                TempData["Success"] = "Create successfully";
            }
            else
            {
                //var error = JsonSerializer.Deserialize<string>(responseBody, options);
                //Console.WriteLine(error);
                //TempData["Error"] = string.Join("\n", error);
                TempData["Error"] = responseBody;
                Console.WriteLine(responseBody);
                return RedirectToAction(nameof(Classes));
            }
            return RedirectToAction(nameof(Classes));
        }

        #region Other Functions
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

        private string BuildQueryStringUsers(string? searchKeyword, int? roleId, bool? disabled, bool? verified, string? sortBy, int? page, int? pageSize)
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

        private string BuildQueryStringClasses(string? searchKeyword, string? sortBy, DateTime? startingFromDate, int? durationMonths, string? classCapacity, int? page, int? pageSize)
        {
            var queryString = "";

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                queryString += $"searchKeyword={searchKeyword}&";
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                queryString += $"sortBy={sortBy}&";
            }

            if (startingFromDate != null)
            {
                queryString += $"startingFromDate={startingFromDate}&";
            }

            if (durationMonths != null)
            {
                queryString += $"durationMonths={durationMonths}&";
            }

            if (!string.IsNullOrEmpty(classCapacity))
            {
                queryString += $"classCapacity={classCapacity}&";
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

        private string SelectDay(CreateClassDto model)
        {
            List<string> dayList = new List<string>();
            if (model.Monday != null)
            {
                dayList.Add("1");
            }
            if (model.Tuesday != null)
            {
                dayList.Add("2");
            }
            if (model.Wednesday != null)
            {
                dayList.Add("3");
            }
            if (model.Thursday != null)
            {
                dayList.Add("4");
            }
            if (model.Friday != null)
            {
                dayList.Add("5");
            }
            if (model.Saturday != null)
            {
                dayList.Add("6");
            }
            if (model.Sunday != null)
            {
                dayList.Add("7");
            }
            return String.Join(",",dayList.ToArray());
        }
    }
    #endregion
}

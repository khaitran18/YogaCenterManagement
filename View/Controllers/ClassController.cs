﻿using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;
using View.Models;
using View.Models.Response;

namespace View.Controllers
{
    public class ClassController : Controller
    {
        private readonly HttpClient _httpClient;
        private string apiUrl = "";
        private string studyingClassApiUrl = "";
        private string changeClassApi = "";
        private string changeClassApi2 = "";
        private string enrollClassApiUrl = "";
        private string teachingClassApiUrl = "";
        private string availableDateApiUrl = "";
        private string slotApiUrl = "";
        public ClassController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiUrl = "https://localhost:7241/api/class";
            studyingClassApiUrl = "https://localhost:7241/api/Class/studyclass";
            changeClassApi = "https://localhost:7241/api/Class/changeclasses";
            changeClassApi2 = "https://localhost:7241/api/Class/changeclass";
            enrollClassApiUrl = "https://localhost:7241/api/Class/enroll";
            teachingClassApiUrl = "https://localhost:7241/api/Class/teachclass";
            availableDateApiUrl = "https://localhost:7241/api/Class/availabledate";
            slotApiUrl = "https://localhost:7241/api/Class/slot";
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

        public async Task<IActionResult> Index(string? searchKeyword, string? sortBy, DateTime? startingFromDate, int? durationMonths, string? classCapacity, int? page, int? pageSize)
        {
            var queryString = BuildQueryStringClasses(searchKeyword, sortBy, startingFromDate, durationMonths, classCapacity, page, pageSize);
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
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var url = $"{apiUrl}/{id}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<ClassDto>>(responseBody, options);

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
            else
            {
                return View();
            }
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


        public async Task<IActionResult> StudyingClasses(int page = 1, int pageSize = 6)
        {
            var studentId = Request.Cookies["Id"];
            Console.WriteLine(studentId);
            var response = await _httpClient.GetAsync($"{studyingClassApiUrl}/{studentId}?page={page}&pageSize={pageSize}");
            Console.WriteLine($"{studyingClassApiUrl}/{studentId}");
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
                    //ViewBag.QueryString = queryStringWithoutPage;
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
                return View();
            }

        }

        public async Task<IActionResult> StudyingClassesById(int classId)
        {
            var studentId = Request.Cookies["Id"];
            var response = await _httpClient.GetAsync($"{studyingClassApiUrl}?StudentId={studentId}&ClassId={classId}");
            var changeClassResponse = await _httpClient.GetAsync($"https://localhost:7241/api/Class/changeclasses/{classId}");
            Console.WriteLine($"{studyingClassApiUrl}?StudentId={studentId}&ClassId={classId}");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var baseResponse = JsonSerializer.Deserialize<BaseResponse<ClassDto>>(responseBody, options);

                if (!baseResponse!.Error)
                {
                    if (changeClassResponse.IsSuccessStatusCode)
                    {
                        var changeClassResponseBody = await changeClassResponse.Content.ReadAsStringAsync();
                        var changeClassBaseResponse = JsonSerializer.Deserialize<BaseResponse<IEnumerable<ClassDto>>>(changeClassResponseBody, options);
                        ViewBag.ChangeClassList = changeClassBaseResponse?.Result;
                    }
                    //ViewBag.QueryString = queryStringWithoutPage;
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
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateChangeRequest(int toClassId, int fromClassId, string content)
        {
            var studentId = Request.Cookies["Id"];
            var requestData = new
            {
                FromClassId = fromClassId,
                ToClassId = toClassId,
                StudentId = studentId,
                Content = content
            };

            var json = JsonSerializer.Serialize(requestData);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(changeClassApi2, stringContent);
            var resultJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine(response);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<BaseResponse<ClassDto>>(resultJson, options);
                if (!result!.Error)
                {
                    if (result.Result.ClassId != 0)
                    {
                        TempData["Success"] = "Request successfully";
                    }
                }
                else
                {
                    TempData["Error"] = result.Message;
                }
            }
            //Console.WriteLine(requestData);
            return RedirectToAction(nameof(StudyingClassesById), new { classId = fromClassId });
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(int classId, decimal amount)
        {
            var studentId = Request.Cookies["Id"];
            var requestData = new
            {
                StudentId = studentId,
                ClassId = classId,
                Amount = amount,
                Method = "Online"
            };

            var json = JsonSerializer.Serialize(requestData);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(enrollClassApiUrl, stringContent);
            var resultJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine(response);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<BaseResponse<PaymentDto>>(resultJson, options);
                if (!result!.Error)
                {
                    if (result.Result.ClassId != 0)
                    {
                        TempData["Success"] = "Enroll successfully";
                    }
                }
                else
                {
                    TempData["Error"] = result.Message;
                }
            }
            //Console.WriteLine(requestData);

            return RedirectToAction(nameof(Details), new { classId });
        }

        public async Task<IActionResult> Schedule([FromQuery] int classId, int s = 0)
        {
            //s is number of shifting
            string url = apiUrl + "/schedule";
            DateTime today = DateTime.Today.AddDays(s * 7);
            int daysUntilMonday = ((int)today.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            DateTime startDate = today.AddDays(-daysUntilMonday).Date; // Beginning of the week (Monday)
            DateTime endDate = startDate.AddDays(6).Date; // End of the week (Sunday)
            AddAuthTokenToRequestHeaders();
            var response = await _httpClient.GetAsync(url + "?classId=" + classId + "&startDate=" + startDate + "&endDate=" + endDate);
            var resultJson = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var baseResponse = JsonSerializer.Deserialize<BaseResponse<List<ScheduleDto>>>(resultJson, options);
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
                TempData["Error"] = errorResponse;
                ViewBag.ErrorResponse = errorResponse;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> TeachingClasses(int page = 1, int pageSize = 6)
        {
            var lecturerId = Request.Cookies["Id"];
            Console.WriteLine(lecturerId);
            Console.WriteLine($"{teachingClassApiUrl}/{lecturerId}?page={page}&pageSize={pageSize}");

            var response = await _httpClient.GetAsync($"{teachingClassApiUrl}/{lecturerId}?page={page}&pageSize={pageSize}");
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
                    //ViewBag.QueryString = queryStringWithoutPage;
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
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> TeachingClassesById(int classId)
        {
            var lecturerId = Request.Cookies["Id"];
            var response = await _httpClient.GetAsync($"{teachingClassApiUrl}?LecturerId={lecturerId}&ClassId={classId}");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var baseResponse = JsonSerializer.Deserialize<BaseResponse<ClassDto>>(responseBody, options);

                if (!baseResponse!.Error)
                {
                    //ViewBag.QueryString = queryStringWithoutPage;
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
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> AvailableDates()
        {
            var lecturerId = Request.Cookies["Id"];

            var response = await _httpClient.GetAsync($"{availableDateApiUrl}/bylecturerid/{lecturerId}");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var baseResponse = JsonSerializer.Deserialize<BaseResponse<IEnumerable<AvailableDateDto>>>(responseBody, options);

                if (!baseResponse!.Error)
                {
                    //ViewBag.QueryString = queryStringWithoutPage;
                    var allAvailableDatesResponse = await _httpClient.GetAsync($"{slotApiUrl}");
                    if (allAvailableDatesResponse.IsSuccessStatusCode)
                    {
                        var allAvailableDatesResponseBody = await allAvailableDatesResponse.Content.ReadAsStringAsync();
                        var allAvailableDatesBaseResponse = JsonSerializer.Deserialize<BaseResponse<IEnumerable<StudySlotDto>>>(allAvailableDatesResponseBody, options);

                        var slotsToAdd = allAvailableDatesBaseResponse?.Result
                                                        .Where(slot => !baseResponse.Result.Any(existingDate => existingDate.Slot.SlotId == slot.SlotId))
                                                        .ToList();

                        ViewBag.AvailableDates = slotsToAdd;
                    }
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
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAvailableDate(List<int> selectedSlots)
        {
            var lecturerId = Request.Cookies["Id"];
            var token = GetAuthTokenFromCookie();
            //foreach (var item in selectedSlots)
            //{
            //    Console.WriteLine(item);
            //}
            var requestData = new { 
                Token = token,
                LecturerId = lecturerId,
                SlotIds = selectedSlots
            };
            var json = JsonSerializer.Serialize(requestData);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(availableDateApiUrl, stringContent);
            var resultJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine(response);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<BaseResponse<IEnumerable<AvailableDateDto>>>(resultJson, options);
                if (!result!.Error)
                {
                    if (result.Result.Count() != 0)
                    {
                        TempData["Success"] = "Enroll successfully";
                    }
                }
                else
                {
                    TempData["Error"] = result.Message;
                }
            }
            //Console.WriteLine(requestData);

            return RedirectToAction(nameof(AvailableDates));
        }

        [HttpPost]
        public async Task<IActionResult> StudySlotsDelete([FromForm] int slotId)
        {
            var response = await _httpClient.DeleteAsync($"{availableDateApiUrl}");
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
                return RedirectToAction(nameof(AvailableDates));
            }
            //Console.WriteLine(baseResponse);

            return RedirectToAction(nameof(AvailableDates));
        }
    }
}

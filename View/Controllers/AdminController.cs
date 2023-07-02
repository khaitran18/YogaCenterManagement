using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using View.Models;
using View.Models.Response;

namespace View.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private string apiUrl = "";
        public AdminController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiUrl = "https://localhost:7241/api/user";
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

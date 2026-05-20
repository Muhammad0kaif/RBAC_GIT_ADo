using Microsoft.AspNetCore.Mvc;
using MVCview.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MVCview.Controllers
{
    public class AuditLogsController : Controller
    {
        public async Task<IActionResult> AuditLogs(int page = 1)
        {
            var role = HttpContext.Session.GetString("role");
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            if (role != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            PagedResult<AuditLogDto> result = new();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                    var response = await client.GetAsync(
                        $"https://localhost:7050/api/audit-logs?page={page}&pageSize=10");

                    var responseText = await response.Content.ReadAsStringAsync();

                    var error = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        ViewBag.Error = responseText;
                        return View(result);
                    }

                    result = JsonSerializer.Deserialize<PagedResult<AuditLogDto>>(
                        error,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }) ?? new PagedResult<AuditLogDto>();
                }

                ViewBag.Page = page;
                ViewBag.TotalPages = (int)Math.Ceiling(result.TotalCount / 10.0);

                return View(result);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "API server is not running. Please start AdoApi2.";
                return View(result);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading audit logs.";
                return View(result);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserLogs(Guid userGuid)
        {
            var role = HttpContext.Session.GetString("role");
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            if (role != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            List<AuditLogDto> logs = new();

            try
            {
                using var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    $"https://localhost:7050/api/audit-logs/{userGuid}");

                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = responseText;
                    return View(logs);
                }

                logs = JsonSerializer.Deserialize<List<AuditLogDto>>(
                    responseText,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<AuditLogDto>();

                ViewBag.UserGuid = userGuid;

                return View(logs);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "API server is not running. Please start AdoApi2.";
                return View(logs);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading user audit logs.";
                return View(logs);
            }
        }
    }
}
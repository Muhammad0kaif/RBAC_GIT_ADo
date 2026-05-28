using Microsoft.AspNetCore.Mvc;
using MVCview.Models;
using MVCview.Services;
using PocoClasses;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MVCview.Controllers
{
    public class UsersController(ApiClientService apiClient) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var permissionsJson = HttpContext.Session.GetString("permissions");

            if (string.IsNullOrEmpty(permissionsJson))
            {
                return RedirectToAction("Login", "Account");
            }

            var permissions =
                JsonSerializer.Deserialize<List<PermissionDto>>(permissionsJson);

            var pagePermission =
                permissions.FirstOrDefault(x =>
                    x.PageName.ToLower() == "users");

            if (pagePermission == null || pagePermission.CanRead == false)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            ViewBag.Permissions = permissions;

            var apiResult = await apiClient.GetAsync<List<UserDto>>("/api/Users/get-users");
            var departmentResult = await apiClient.GetAsync<List<DepartmentDto>>("/api/departments");

            ViewBag.Departments = departmentResult.Success
                ? departmentResult.Data
                : new List<DepartmentDto>();
            if (!apiResult.Success)
            {
                ViewBag.Error = apiResult.Error;
                return View(new List<UserDto>());
            }

            return View(apiResult.Data ?? new List<UserDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDto model)
        {

            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var apiResult = await apiClient.PostAsync<string>( "/api/Users/create-user", model);

            if (apiResult.Success)
            {
                TempData["Success"] = "User created successfully.";
                return RedirectToAction("Users");
            }

            ViewBag.Error = apiResult.Error;
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var permissionsJson = HttpContext.Session.GetString("permissions");

            if (string.IsNullOrEmpty(permissionsJson))
                return RedirectToAction("Login", "Account");

            var permissions = JsonSerializer.Deserialize<List<PermissionDto>>(permissionsJson);

            var pagePermission = permissions.FirstOrDefault(x => x.PageName.ToLower() == "users");

            if (pagePermission == null || pagePermission.CanWrite == false)
                return RedirectToAction("AccessDenied", "Home");

            List<Role> roles = new();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7050/api/auth/roles");

                if (response.IsSuccessStatusCode)
                {
                    roles = await response.Content.ReadFromJsonAsync<List<Role>>();
                }
            }
            var departmentResult = await apiClient.GetAsync<List<DepartmentDto>>("/api/departments");

            ViewBag.Departments = departmentResult.Success ? departmentResult.Data: new List<DepartmentDto>();
            ViewBag.Roles = roles ?? new List<Role>();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var apiResult = await apiClient.DeleteAsync<string>($"/api/Users/delete-user/{id}");

            if (apiResult.Success)
            {
                TempData["Success"] = "User deleted successfully.";
            }
            else
            {
                TempData["Error"] = apiResult.Error;
            }

            return RedirectToAction("Users");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("token");

            UserDto user = null;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    "https://localhost:7050/api/Users/get-users");

                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content
                        .ReadFromJsonAsync<List<UserDto>>();

                    user = users.FirstOrDefault(x => x.Id == id);
                }
            }

            var departmentResult = await apiClient.GetAsync<List<DepartmentDto>>("/api/departments");

            ViewBag.Departments = departmentResult.Success
                ? departmentResult.Data
                : new List<DepartmentDto>();
            if (user == null)
            {
                ViewBag.Error = "User not found";
                return RedirectToAction("Users");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDto model)
        {
            var token = HttpContext.Session.GetString("token");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PutAsJsonAsync(
                    $"https://localhost:7050/api/Users/update-user/{model.Id}",
                    model);

                var error = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Users");
                }

                ViewBag.Error = error;
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Unlock(Guid id)
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var apiResult = await apiClient.PostAsync<string>(
                $"/api/auth/unlock/{id}",
                new { });

            if (apiResult.Success)
            {
                TempData["Success"] = "User unlocked successfully.";
            }
            else
            {
                TempData["Error"] = apiResult.Error;
            }

            return RedirectToAction("Users");
        }


        [HttpGet]
        public async Task<IActionResult> PasswordHistory(Guid id)
        {
            var token = HttpContext.Session.GetString("token");
            var role = HttpContext.Session.GetString("role");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            if (role != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var apiResult = await apiClient.GetAsync<List<PasswordHistoryDto>>(
                $"/api/Users/{id}/password-history");

            if (!apiResult.Success)
            {
                TempData["Error"] = apiResult.Error;
                return RedirectToAction("Users");
            }

            return View(apiResult.Data ?? new List<PasswordHistoryDto>());
        }

        [HttpPost]
        public async Task<IActionResult> TransferDepartment(Guid userId, Guid departmentId)
        {
            var token = HttpContext.Session.GetString("token");
            var role = HttpContext.Session.GetString("role");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            if (role != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var apiResult = await apiClient.PutAsync<string>(
                $"/api/Users/{userId}/transfer-department/{departmentId}",
                new { });

            if (apiResult.Success)
            {
                TempData["Success"] = "User transferred successfully.";
            }
            else
            {
                TempData["Error"] = apiResult.Error;
            }

            return RedirectToAction("Users");
        }
    }
}

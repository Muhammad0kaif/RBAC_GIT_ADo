using Microsoft.AspNetCore.Mvc;
using MVCview.Models;
using System.Data;
using System.Net.Http.Headers;
using System.Text.Json;
using PocoClasses;

namespace MVCview.Controllers
{
    public class UsersController : Controller
    {
        


        [HttpGet]
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

            var token = HttpContext.Session.GetString("token");

            List<UserDto> users = new();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    "https://localhost:7050/api/Users/get-users");

                var responseText = await response.Content.ReadAsStringAsync();

                Console.WriteLine("USERS API STATUS => " + response.StatusCode);
                Console.WriteLine("USERS API RESPONSE => " + responseText);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = responseText;
                    return View(new List<UserDto>());
                }

                users = JsonSerializer.Deserialize<List<UserDto>>(
                    responseText,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                ) ?? new List<UserDto>();
            }

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDto model)
        {

            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                using var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsJsonAsync(
                    "https://localhost:7050/api/Users/create-user",
                    model);

                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "User created successfully.";
                    return RedirectToAction("Users");
                }

                ViewBag.Error = responseText;
                return View(model);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "API server is not running. Please start AdoApi2.";
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while creating user.";
                return View(model);
            }
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

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.DeleteAsync(
                    $"https://localhost:7050/api/Users/delete-user/{id}");
                var responseText = await response.Content.ReadAsStringAsync();

                var error = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "User deleted successfully.";
                }
                else
                {
                    TempData["Error"] = responseText;
                }
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "API server is not running. Please start AdoApi2.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Something went wrong while deleting user.";
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

    }
}

using Microsoft.AspNetCore.Mvc;
using MVCview.Models;
using MVCview.Services;

namespace MVCview.Controllers
{
    public class DepartmentsController(ApiClientService apiClient) : Controller
    {
        public async Task<IActionResult> Departments()
        {
            var token = HttpContext.Session.GetString("token");
            var role = HttpContext.Session.GetString("role");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            if (role != "Admin" && role != "Manager")
            {
                return RedirectToAction("Index", "Home");
            }

            var apiResult = await apiClient.GetAsync<List<DepartmentDto>>(
                "/api/departments");

            if (!apiResult.Success)
            {
                ViewBag.Error = apiResult.Error;
                return View(new List<DepartmentDto>());
            }

            return View(apiResult.Data ?? new List<DepartmentDto>());
        }

        public async Task<IActionResult> Users(Guid id)
        {
            var token = HttpContext.Session.GetString("token");
            var role = HttpContext.Session.GetString("role");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            if (role != "Admin" && role != "Manager")
            {
                return RedirectToAction("Index", "Home");
            }

            var apiResult = await apiClient.GetAsync<List<UserDto>>(
                $"/api/departments/{id}/users");

            if (!apiResult.Success)
            {
                TempData["Error"] = apiResult.Error;
                return RedirectToAction("Departments");
            }

            ViewBag.DepartmentId = id;

            return View(apiResult.Data ?? new List<UserDto>());
        }
    }
}
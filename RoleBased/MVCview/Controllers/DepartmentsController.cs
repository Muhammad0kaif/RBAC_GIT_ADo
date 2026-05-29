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

            var departments = apiResult.Data ?? new List<DepartmentDto>();

            if (role == "Manager")
            {
                var departmentIdText = HttpContext.Session.GetString("DepartmentId");

                if (Guid.TryParse(departmentIdText, out Guid managerDepartmentId))
                {
                    departments = departments
                        .Where(x => x.Id == managerDepartmentId)
                        .ToList();
                }
                else
                {
                    departments = new List<DepartmentDto>();
                    ViewBag.Error = "Your department is not assigned. Please contact admin.";
                }
            }

            return View(departments);
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
                TempData["Error"] = "You are not allowed to view users from this department.";
                return RedirectToAction("Departments");
            }

            ViewBag.DepartmentId = id;

            return View(apiResult.Data ?? new List<UserDto>());
        }
    }
}
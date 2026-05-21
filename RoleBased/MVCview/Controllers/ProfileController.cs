using Microsoft.AspNetCore.Mvc;
using MVCview.Services;
using PocoClasses.Dto;


namespace MVCview.Controllers
{
    public class ProfileController(ApiClientService apiClient) : Controller
    {
        public async Task<IActionResult> Profile()
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var apiResult = await apiClient.GetAsync<UserDto>(
                "/api/Users/profile");

            if (!apiResult.Success)
            {
                TempData["Error"] = apiResult.Error;
                return RedirectToAction("Login", "Account");
            }

            return View(apiResult.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var apiResult = await apiClient.GetAsync<UserDto>(
                "/api/Users/profile");

            if (!apiResult.Success)
            {
                TempData["Error"] = apiResult.Error;
                return RedirectToAction("Profile");
            }

            return View(apiResult.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDto model)
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var apiResult = await apiClient.PutAsync<string>(
                "/api/Users/update-profile",
                model);

            if (apiResult.Success)
            {
                TempData["Success"] = "Profile updated successfully.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = apiResult.Error;
            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ViewBag.Error = "Password and Confirm Password do not match";
                return View(model);
            }

            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var apiResult = await apiClient.PostAsync<string>(
                "/api/Users/change-password",
                model);

            if (apiResult.Success)
            {
                HttpContext.Session.SetString("mustChangePassword", "False");

                TempData["Success"] = "Password changed successfully.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = apiResult.Error;
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(Guid userId, IFormFile file)
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a file.";
                return RedirectToAction("Profile");
            }

            var apiResult = await apiClient.UploadFileAsync(
                $"/api/Users/{userId}/upload",
                file);

            if (apiResult.Success)
            {
                TempData["Success"] = "Profile picture updated successfully.";
                return RedirectToAction("Profile");
            }

            TempData["Error"] = apiResult.Error;
            return RedirectToAction("Profile");
        }

    }
}
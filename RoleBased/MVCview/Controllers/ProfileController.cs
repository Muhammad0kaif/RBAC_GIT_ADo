using Microsoft.AspNetCore.Mvc;
using PocoClasses.Dto;
using System.Net.Http.Headers;

namespace MVCview.Controllers
{
    public class ProfileController : Controller
    {
        public async Task<IActionResult> Profile()
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            UserDto user = null;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    "https://localhost:7050/api/Users/profile");

                if (response.IsSuccessStatusCode)
                {
                    user = await response.Content.ReadFromJsonAsync<UserDto>();
                }
            }

            if (user == null)
                return RedirectToAction("Login", "Account");

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            UserDto user = null;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    "https://localhost:7050/api/Users/profile");

                if (response.IsSuccessStatusCode)
                {
                    user = await response.Content.ReadFromJsonAsync<UserDto>();
                }
            }

            if (user == null)
                return RedirectToAction("Login", "Account");

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDto model)
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

                var response = await client.PutAsJsonAsync(
                    "https://localhost:7050/api/Users/update-profile",
                    model);

                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Profile updated successfully.";
                    return RedirectToAction("Index", "Home");
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
                ViewBag.Error = "Something went wrong while updating profile.";
                return View(model);
            }
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

            try
            {
                using var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsJsonAsync(
                    "https://localhost:7050/api/Users/change-password",
                    model);

                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("mustChangePassword", "False");

                    TempData["Success"] = "Password changed successfully.";
                    return RedirectToAction("Index", "Home");
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
                ViewBag.Error = "Something went wrong while changing password.";
                return View(model);
            }
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

            try
            {
                using var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                using var form = new MultipartFormDataContent();

                using var stream = file.OpenReadStream();

                var fileContent = new StreamContent(stream);

                fileContent.Headers.ContentType =
                    new MediaTypeHeaderValue(file.ContentType);

                form.Add(fileContent, "file", file.FileName);

                var response = await client.PostAsync(
                    $"https://localhost:7050/api/Users/{userId}/upload",
                    form);

                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Profile picture updated successfully.";
                    return RedirectToAction("Profile");
                }

                TempData["Error"] = responseText;
                return RedirectToAction("Profile");
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "API server is not running. Please start AdoApi2.";
                return RedirectToAction("Profile");
            }
            catch (Exception)
            {
                TempData["Error"] = "Something went wrong while uploading profile picture.";
                return RedirectToAction("Profile");
            }
        }
    }
}
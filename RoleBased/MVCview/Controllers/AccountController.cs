using Microsoft.AspNetCore.Mvc;
using MVCview.Models;
using System.Text.Json;

namespace MVCview.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                using var client = new HttpClient();

                var response = await client.PostAsJsonAsync(
                    "https://localhost:7050/api/auth/login",
                    model);

                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = responseText;
                    return View(model);
                }

                var result = await response.Content
                    .ReadFromJsonAsync<LoginResponse>();

                if (result == null)
                {
                    ViewBag.Error = "Invalid response from API.";
                    return View(model);
                }

                HttpContext.Session.SetString("token", result.AccessToken);
                HttpContext.Session.SetString("refreshToken", result.RefreshToken);
                HttpContext.Session.SetString("role", result.role.ToString());
                HttpContext.Session.SetString("UserId", result.UserId.ToString());
                HttpContext.Session.SetString(
                    "permissions",
                    JsonSerializer.Serialize(result.permissions));

                HttpContext.Session.SetString(
                    "mustChangePassword",
                    result.MustChangePassword.ToString());

                if (result.MustChangePassword)
                {
                    return RedirectToAction("ChangePassword", "Profile");
                }

                return RedirectToAction("Index", "Home");
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "API server is not running. Please start AdoApi2.";
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while login.";
                return View(model);
            }
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }




        public IActionResult Logout()
        {
           HttpContext.Session.Clear();
           return RedirectToAction("Login", "Account");
        }




        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                using var client = new HttpClient();

                var response = await client.PostAsJsonAsync(
                    "https://localhost:7050/api/auth/register",
                    model);

                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Registration successful. Please login.";
                    return RedirectToAction("Login");
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
                ViewBag.Error = "Something went wrong while registering.";
                return View(model);
            }
        }
    }
}

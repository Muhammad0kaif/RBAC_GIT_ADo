using Microsoft.AspNetCore.Mvc;
using MVCview.Models;
using MVCview.Services;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace MVCview.Controllers
{
    public class OrdersController : Controller
    {

        private readonly TokenService tokenService;
        public OrdersController(TokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public async Task<IActionResult> Orders(int page = 1)
        {

            var token = HttpContext.Session.GetString("token");

            PagedResult<OrderDto> result = new();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    $"https://localhost:7050/api/orders/get-orders?page={page}&pageSize=5");


                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var refreshed =
                        await tokenService.RefreshAccessToken();

                    if (refreshed)
                    {
                        var newToken =
                            HttpContext.Session.GetString("token");

                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue(
                                "Bearer",
                                newToken);

                        response = await client.GetAsync(
                            $"https://localhost:7050/api/orders/get-orders?page={page}&pageSize=5");
                    }
                }


                var responseText = await response.Content.ReadAsStringAsync();

                Console.WriteLine("ORDERS API STATUS => " + response.StatusCode);
                Console.WriteLine("ORDERS API RESPONSE => " + responseText);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = responseText;
                    return View(new List<OrderDto>());
                }

                result = System.Text.Json.JsonSerializer.Deserialize<PagedResult<OrderDto>>(
                    responseText,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new PagedResult<OrderDto>();
            }

            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(result.TotalCount / 5.0);
            if (result == null)
            {
                result = new PagedResult<OrderDto>
                {
                    Items = new List<OrderDto>(),
                    TotalCount = 0
                };
            }
            return View(result?.Items ?? new List<OrderDto>());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(OrderDto model)
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            model.UserId = string.IsNullOrEmpty(userIdString)? Guid.Empty: Guid.Parse(userIdString);

            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                ViewBag.Error = "Invalid user session. Please login again.";
                return View(model);
            }

            model.UserId = userId;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                    var response = await client.PostAsJsonAsync(
                        "https://localhost:7050/api/orders/create-order",
                        model);
                    var error = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Orders");
                    }
                    ViewBag.Error = error;
                }

                return View(model);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "API server is not running. Please start AdoApi2.";
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while creating order.";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            PagedResult<OrderDto> result = new();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    "https://localhost:7050/api/orders/get-orders?page=1&pageSize=100");

                var error = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = error;
                    return RedirectToAction("Orders");
                }

                result = await response.Content
                    .ReadFromJsonAsync<PagedResult<OrderDto>>();
            }

            var order = result?.Items?.FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                TempData["Error"] = "Order not found";
                return RedirectToAction("Orders");
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderDto model)
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
                    $"https://localhost:7050/api/orders/update-order/{model.Id}",
                    model);

                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Order updated successfully.";
                    return RedirectToAction("Orders");
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
                ViewBag.Error = "Something went wrong while updating order.";
                return View(model);
            }
        }
    }
}



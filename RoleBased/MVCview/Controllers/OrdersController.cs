using Microsoft.AspNetCore.Mvc;
using MVCview.Models;
using MVCview.Services;

namespace MVCview.Controllers
{
    public class OrdersController(ApiClientService apiClient) : Controller
    {
        public async Task<IActionResult> Orders(int page = 1)
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var apiResult = await apiClient.GetAsync<PagedResult<OrderDto>>(
                $"/api/orders/get-orders?page={page}&pageSize=5");

            if (!apiResult.Success)
            {
                ViewBag.Error = apiResult.Error;
                ViewBag.Page = page;
                ViewBag.TotalPages = 1;

                return View(new List<OrderDto>());
            }

            var result = apiResult.Data ?? new PagedResult<OrderDto>();

            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(result.TotalCount / 5.0);

            return View(result.Items ?? new List<OrderDto>());
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

            var apiResult = await apiClient.PostAsync<string>(
                "/api/orders/create-order",
                model);

            if (apiResult.Success)
            {
                TempData["Success"] = "Order created successfully.";
                return RedirectToAction("Orders");
            }

            ViewBag.Error = apiResult.Error;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var apiResult = await apiClient.GetAsync<PagedResult<OrderDto>>(
                "/api/orders/get-orders?page=1&pageSize=100");

            if (!apiResult.Success)
            {
                TempData["Error"] = apiResult.Error;
                return RedirectToAction("Orders");
            }

            var order = apiResult.Data?.Items?.FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                TempData["Error"] = "Order not found.";
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

            var apiResult = await apiClient.PutAsync<string>(
                $"/api/orders/update-order/{model.Id}",
                model);

            if (apiResult.Success)
            {
                TempData["Success"] = "Order updated successfully.";
                return RedirectToAction("Orders");
            }

            ViewBag.Error = apiResult.Error;
            return View(model);
        }
    }
}
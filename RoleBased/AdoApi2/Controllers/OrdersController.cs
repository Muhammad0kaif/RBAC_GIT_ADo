using AdoApi2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocoClasses;
using PocoClasses.Dto;
using System.Security.Claims;

namespace AdoApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(OrderService service) : ControllerBase
    {
        [Authorize(Roles = "Admin,User")]
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(OrderDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                ProductName = dto.ProductName,
                Quantity = dto.Quantity,
                Price = dto.Price,
                UserId = userId
            };

            await service.CreateOrder(order);

            return Ok("Order Created");
        }

        [Authorize]
        [HttpPut("update-order/{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, OrderDto dto)
        {
            var order = new Order
            {
                Id = id,
                ProductName = dto.ProductName,
                Quantity = dto.Quantity,
                Price = dto.Price
            };

            await service.UpdateOrder(order);

            return Ok("Order Updated");
        }

        [Authorize]
        [HttpGet("get-orders")]
        public async Task<IActionResult> GetOrders(int page = 1, int pageSize = 5)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("Invalid user id in token");
            }

            bool isAdmin = role == "Admin";

            var result = await service.GetOrders(userId, isAdmin, page, pageSize);

            Console.WriteLine("ROLE => " + role);
            Console.WriteLine("USER ID => " + userId);
            Console.WriteLine("IS ADMIN => " + isAdmin);
            Console.WriteLine("ORDER COUNT => " + result.Item1.Count);
            Console.WriteLine("TOTAL COUNT => " + result.Item2);

            return Ok(new
            {
                totalCount = result.Item2,
                items = result.Item1
            });
        }
    }
}
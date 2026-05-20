using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdoApi2.Services;
using PocoClasses.Dto;

namespace AdoApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController(ReportService service) : ControllerBase
    {
        [Authorize]
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var (TotalOrders, TotalSales) = await service.GetDashboard();

            return Ok(new ReportDto
            {
                TotalOrders = TotalOrders,
                TotalSales = TotalSales
            });
        }
    }
}
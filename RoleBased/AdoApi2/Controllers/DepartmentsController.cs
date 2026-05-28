using AdoApi2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdoApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController(DepartmentService service) : ControllerBase
    {
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            return Ok(await service.GetDepartments());
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetUsersByDepartment(Guid id)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var departmentClaim = User.FindFirst("DepartmentId")?.Value;

            if (role == "Manager")
            {
                if (!Guid.TryParse(departmentClaim, out Guid managerDepartmentId))
                    return Forbid();

                if (managerDepartmentId != id)
                    return Forbid();
            }

            var users = await service.GetUsersByDepartment(id);

            return Ok(users);
        }
    }
}
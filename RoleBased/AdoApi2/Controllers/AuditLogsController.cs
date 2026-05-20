using AdoApi2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdoApi2.Controllers
{
    [Route("api/audit-logs")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuditLogsController(AuditLogService auditLogService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAuditLogs(
            int page = 1,
            int pageSize = 10)
        {
            var result =
                await auditLogService.GetAuditLogsPaged(page, pageSize);

            return Ok(new
            {
                items = result.Logs,
                totalCount = result.TotalCount,
                page,
                pageSize
            });
        }

        [HttpGet("{userGuid}")]
        public async Task<IActionResult> GetAuditLogsByUser(Guid userGuid)
        {
            var logs =
                await auditLogService.GetAuditLogsByUser(userGuid);

            return Ok(logs);
        }
    }
}
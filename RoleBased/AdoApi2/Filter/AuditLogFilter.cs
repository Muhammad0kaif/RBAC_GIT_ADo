using AdoApi2.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using PocoClasses.Dto;
using System.Security.Claims;

namespace AdoApi2.Filters
{
    public class AuditLogFilter(AuditLogService auditLogService)
        : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var httpMethod =
                context.HttpContext.Request.Method;

            bool shouldLog =
                httpMethod == "POST" ||
                httpMethod == "PUT" ||
                httpMethod == "DELETE";

            var executedContext = await next();

            if (!shouldLog)
                return;

            if (executedContext.Exception != null)
                return;

            var userIdClaim =
                context.HttpContext.User.FindFirst(
                    ClaimTypes.NameIdentifier)?.Value;

            Guid? userId = null;

            if (Guid.TryParse(userIdClaim, out Guid parsedUserId))
            {
                userId = parsedUserId;
            }

            var controller =
                context.RouteData.Values["controller"]?.ToString();

            var action =
                context.RouteData.Values["action"]?.ToString();

            var ip =
                context.HttpContext.Connection.RemoteIpAddress?.ToString();

            var log = new AuditLogDto
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Action = $"{httpMethod} {controller}/{action}",
                Timestamp = DateTime.UtcNow,
                IP = ip
            };

            await auditLogService.InsertAuditLog(log);
        }
    }
}
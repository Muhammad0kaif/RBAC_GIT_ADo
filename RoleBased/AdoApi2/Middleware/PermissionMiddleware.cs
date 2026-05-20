using AdoApi2.Infrastructure;
using AdoApi2.Repositories.Interfaces;
using AdoApi2.Services;
using System.Security.Claims;

namespace AdoApi2.Middleware
{
    public class PermissionMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context, IPermissionRepository repo)
        {
            var endpoint = context.Request.Path.Value?.ToLower() ?? "";

            if (endpoint.Contains("auth") || endpoint.Contains("permissions"))
            {
                await next(context);
                return;
            }

            var roleClaim = context.User.FindFirst(ClaimTypes.Role);
            var roleIdClaim = context.User.FindFirst("RoleId");

            if (roleClaim == null || roleIdClaim == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var roleId = int.Parse(roleIdClaim.Value);

            var segments = endpoint.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (segments.Length < 2)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Invalid endpoint");
                return;
            }

            var pageName = segments[1];

            var hasPermission = await repo.HasPermission(roleId, pageName);

            if (!hasPermission)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Access Denied");
                return;
            }

            await next(context);
        }
    }
}
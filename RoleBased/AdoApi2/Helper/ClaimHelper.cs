using System.Security.Claims;

namespace AdoApi2.Helper
{
    public static class ClaimHelper
    {
        public static Guid GetUserId(ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }

        public static string GetRole(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)!.Value;
        }

        public static int GetRoleId(ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst("RoleId")!.Value);
        }
    }
}
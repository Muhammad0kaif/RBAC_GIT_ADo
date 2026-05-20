using AdoApi2.Repositories.Interfaces;

namespace AdoApi2.Services
{
    public class PermissionService(IPermissionRepository repo)
    {
        public Task<bool> HasPermission(int roleId, string pageName)
        {
            return repo.HasPermission(roleId, pageName);
        }
    }
}
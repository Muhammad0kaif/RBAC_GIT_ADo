namespace AdoApi2.Repositories.Interfaces
{
    public interface IPermissionRepository
    {
        Task<bool> HasPermission(int roleId, string pageName);
    }
}
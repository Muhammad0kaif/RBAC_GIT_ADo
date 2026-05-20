using PocoClasses;
using PocoClasses.Dto;
using PocoClasses.PocoClasses;

namespace AdoApi2.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> GetUserByEmail(string email);

        Task RegisterUser(User user);

        Task SaveRefreshToken(string token, DateTime expiry, Guid userId);

        Task RevokeRefreshToken(string token);

        Task<RefreshToken> GetRefreshToken(string token);

        Task<Role> GetRoleById(int roleId);
        Task<User> GetUserById(Guid id);
        Task<List<PermissionDto>> GetPermissionsByRoleId(int roleId);
        Task<List<Role>> GetRoles();
    }
}

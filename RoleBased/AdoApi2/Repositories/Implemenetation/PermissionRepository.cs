using AdoApi2.Infrastructure;
using AdoApi2.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace AdoApi2.Repositories.Implemenetation
{
    public class PermissionRepository : BaseRepository, IPermissionRepository
    {
        public PermissionRepository(DbConnectionFactory factory) : base(factory) { }

        public async Task<bool> HasPermission(int roleId, string pageName)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_HasPermission", conn);

            cmd.Parameters.AddWithValue("@RoleId", roleId);
            cmd.Parameters.AddWithValue("@PageName", pageName);

            await conn.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();

            return result != null && Convert.ToBoolean(result);
        }
    }
}
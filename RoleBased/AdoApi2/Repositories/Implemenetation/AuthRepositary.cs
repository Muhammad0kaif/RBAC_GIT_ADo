using AdoApi2.Infrastructure;
using AdoApi2.Repositories.Interfaces;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using PocoClasses;
using PocoClasses.Dto;
using PocoClasses.PocoClasses;

namespace AdoApi2.Repositories.Implemenetation
{
    public class AuthRepository(DbConnectionFactory factory) : BaseRepository(factory), IAuthRepository
    {

        #region Get User By Email

        public async Task<User> GetUserByEmail(string email)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_GetUserByEmail", conn);

            cmd.Parameters.AddWithValue("@Email", email);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader["Id"] == DBNull.Value? Guid.Empty : Guid.Parse(reader["Id"].ToString()!),
                    Name = reader["Name"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Password = reader["PasswordHash"].ToString()!,
                    RoleId = Convert.ToInt32(reader["RoleId"]),
                    MustChangePassword = Convert.ToBoolean(reader["MustChangePassword"]),

                    ProfilePicture = reader["ProfilePicture"] == DBNull.Value ? null : reader["ProfilePicture"].ToString(),
                    FailedLoginAttempts = Convert.ToInt32(reader["FailedLoginAttempts"]),
                    IsLocked = Convert.ToBoolean(reader["IsLocked"]),
                    LockedAt = reader["LockedAt"] == DBNull.Value ? null : Convert.ToDateTime(reader["LockedAt"]),
                    DepartmentId = reader["DepartmentId"] == DBNull.Value ? null : Guid.Parse(reader["DepartmentId"].ToString()!),

                };
            }

            return null;
        }

        #endregion

        #region Register User

        public async Task RegisterUser(User user)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_RegisterUser", conn);

            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", user.Password);
            cmd.Parameters.AddWithValue("@RoleId", user.RoleId);

            await ExecuteNonQuery(cmd);
        }

        #endregion

        #region Refresh Token

        public async Task SaveRefreshToken(string token, DateTime expiry, Guid userId)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_InsertRefreshToken", conn);

            cmd.Parameters.AddWithValue("@Token", token);
            cmd.Parameters.AddWithValue("@ExpiryDate", expiry);
            cmd.Parameters.AddWithValue("@UserId", userId);

            await ExecuteNonQuery(cmd);
        }

        #endregion

        #region Get Refresh Token

        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_GetRefreshToken", conn);

            cmd.Parameters.AddWithValue("@Token", token);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new RefreshToken
                {
                    Token = reader["Token"].ToString()!,
                    UserId = Guid.Parse(reader["UserId"].ToString()!),
                    ExpiryDate = Convert.ToDateTime(reader["ExpiryDate"]),
                    IsRevoked = Convert.ToBoolean(reader["IsRevoked"])
                };
            }

            return null;
        }

        #endregion

        #region Revoke Token

        public async Task RevokeRefreshToken(string token)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_RevokeRefreshToken", conn);

            cmd.Parameters.AddWithValue("@Token", token);

            await ExecuteNonQuery(cmd);
        }

        public async Task<Role> GetRoleById(int roleId)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_GetRoleById", conn);

            cmd.Parameters.AddWithValue("@Id", roleId);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Role
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    RoleName = reader["RoleName"]?.ToString() ?? ""
                };
            }

            return null;
        }

        #endregion

        #region Get User By Id
        public async Task<User> GetUserById(Guid id)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_GetUserById", conn);

            cmd.Parameters.AddWithValue("@Id", id);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = Guid.Parse(reader["Id"].ToString()),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Password = reader["PasswordHash"].ToString(),
                    RoleId = Convert.ToInt32(reader["RoleId"]),
                    MustChangePassword = Convert.ToBoolean(reader["MustChangePassword"])
                };
            }

            return null;
        }

        #endregion

        #region Get Roles
        public async Task<List<Role>> GetRoles()
        {
            List<Role> roles = new();

            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_GetRoles", conn);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                roles.Add(new Role
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    RoleName = reader["RoleName"].ToString()
                });
            }

            return roles;
        }

        #endregion

        #region Get Permission By RoleID
        public async Task<List<PermissionDto>> GetPermissionsByRoleId(int roleId)
        {
            List<PermissionDto> permissions = new();

            using var conn = CreateConnection();

            using var cmd =
                CreateCommand("sp_GetPermissionsByRoleId", conn);

            cmd.Parameters.AddWithValue("@RoleId", roleId);

            await conn.OpenAsync();

            using var reader =
                await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                permissions.Add(new PermissionDto
                {
                    PageName =
                        reader["PageName"].ToString(),

                    CanRead =
                        Convert.ToBoolean(reader["CanRead"]),

                    CanWrite =
                        Convert.ToBoolean(reader["CanWrite"])
                });
            }

            return permissions;
        }

        #endregion

        #region IncreaseFailedLogin
        public async Task IncreaseFailedLogin(Guid userId)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_IncreaseFailedLogin", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);

            await ExecuteNonQuery(cmd);
        }

        #endregion

        #region  ResetFailedLogin
        public async Task ResetFailedLogin(Guid userId)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_ResetFailedLogin", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);

            await ExecuteNonQuery(cmd);
        }

        #endregion

        #region UnlockUser
        public async Task UnlockUser(Guid userId)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_UnlockUser", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);

            await ExecuteNonQuery(cmd);
        }

        #endregion
    }
}
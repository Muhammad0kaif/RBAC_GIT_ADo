using AdoApi2.Infrastructure;
using AdoApi2.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using PocoClasses;
using PocoClasses.PocoClasses;

namespace AdoApi2.Repositories.Implemenetation
{
    public class UserRepository(DbConnectionFactory factory) : BaseRepository(factory), IUserRepository
    {
        public async Task<List<User>> GetUsers()
        {
            var list = new List<User>();

            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_GetUsers", conn);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new User
                {
                    Id = Guid.Parse(reader["Id"].ToString()!),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    RoleId = Convert.ToInt32(reader["RoleId"]),
                    ProfilePicture = reader["ProfilePicture"] == DBNull.Value ? null : reader["ProfilePicture"].ToString()
                });
            }

            return list;
        }

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
                    Id = Guid.Parse(reader["Id"].ToString()!),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    RoleId = Convert.ToInt32(reader["RoleId"]),
                    ProfilePicture = reader["ProfilePicture"] == DBNull.Value? null : reader["ProfilePicture"].ToString()
                };
            }

            return null;
        }

        public async Task CreateUser(User user)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_CreateUser", conn);

            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", user.Password);
            cmd.Parameters.AddWithValue("@RoleId", user.RoleId);
            cmd.Parameters.AddWithValue("@MustChangePassword", user.MustChangePassword);
            await ExecuteNonQuery(cmd);
        }

        public async Task UpdateUser(User user)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_UpdateUser", conn);

            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", user.Password);
            cmd.Parameters.AddWithValue("@RoleId", user.RoleId);

            await ExecuteNonQuery(cmd);
        }

        public async Task DeleteUser(Guid id)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_DeleteUser", conn);

            cmd.Parameters.AddWithValue("@Id", id);

            await ExecuteNonQuery(cmd);
        }

        public async Task UpdateProfilePicture(Guid userId, string filePath)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_UpdateProfilePicture", conn);

            cmd.Parameters.AddWithValue("@Id", userId);
            cmd.Parameters.AddWithValue("@ProfilePicture", filePath);

            await ExecuteNonQuery(cmd);
        }

        public async Task UpdatePassword( Guid userId, string passwordHash,bool mustChangePassword)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_UpdatePassword", conn);

            cmd.Parameters.AddWithValue("@Id", userId);
            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
            cmd.Parameters.AddWithValue("@MustChangePassword", mustChangePassword);

            await ExecuteNonQuery(cmd);
        }
    }
}
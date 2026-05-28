using AdoApi2.Infrastructure;
using AdoApi2.Repositories.Interfaces;
using PocoClasses;
using PocoClasses.Dto;

namespace AdoApi2.Repositories.Implemenetation
{
    public class DepartmentRepository(DbConnectionFactory factory)
        : BaseRepository(factory), IDepartmentRepository
    {
        public async Task<List<DepartmentDto>> GetDepartments()
        {
            List<DepartmentDto> departments = new();

            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_GetDepartments", conn);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                departments.Add(new DepartmentDto
                {
                    Id = Guid.Parse(reader["Id"].ToString()!),
                    Name = reader["Name"].ToString()!
                });
            }

            return departments;
        }

        public async Task<List<User>> GetUsersByDepartment(Guid departmentId)
        {
            List<User> users = new();

            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_GetUsersByDepartment", conn);

            cmd.Parameters.AddWithValue("@DepartmentId", departmentId);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = Guid.Parse(reader["Id"].ToString()!),

                    Name = reader["Name"].ToString()!,

                    Email = reader["Email"].ToString()!,

                    RoleId = Convert.ToInt32(reader["RoleId"]),

                    DepartmentId = reader["DepartmentId"] == DBNull.Value
                        ? null
                        : Guid.Parse(reader["DepartmentId"].ToString()!),

                    MustChangePassword = Convert.ToBoolean(reader["MustChangePassword"]),

                    ProfilePicture = reader["ProfilePicture"] == DBNull.Value
                        ? null
                        : reader["ProfilePicture"].ToString(),

                    FailedLoginAttempts = Convert.ToInt32(reader["FailedLoginAttempts"]),

                    IsLocked = Convert.ToBoolean(reader["IsLocked"]),

                    LockedAt = reader["LockedAt"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["LockedAt"])
                });
            }

            return users;
        }
    }
}
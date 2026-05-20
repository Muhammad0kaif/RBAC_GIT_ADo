using AdoApi2.Infrastructure;
using AdoApi2.Repositories.Interfaces;
using PocoClasses.Dto;

namespace AdoApi2.Repositories.Implemenetation
{
    public class AuditLogRepository(DbConnectionFactory factory)
        : BaseRepository(factory), IAuditLogRepository
    {
        public async Task InsertAuditLog(AuditLogDto dto)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_InsertAuditLog", conn);

            cmd.Parameters.AddWithValue("@Id", dto.Id);
            cmd.Parameters.AddWithValue("@UserId", dto.UserId ?? Guid.Empty);
            cmd.Parameters.AddWithValue("@Action", dto.Action);
            cmd.Parameters.AddWithValue("@Timestamp", dto.Timestamp);
            cmd.Parameters.AddWithValue("@IP", dto.IP ?? "");

            await ExecuteNonQuery(cmd);
        }

        public async Task<(List<AuditLogDto> Logs, int TotalCount)>
            GetAuditLogsPaged(int page, int pageSize)
        {
            List<AuditLogDto> logs = new();
            int totalCount = 0;

            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_GetAuditLogsPaged", conn);

            cmd.Parameters.AddWithValue("@Page", page);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                logs.Add(new AuditLogDto
                {
                    Id = Guid.Parse(reader["Id"].ToString()!),
                    UserId = Guid.Parse(reader["UserId"].ToString()!),
                    Action = reader["Action"].ToString()!,
                    Timestamp = Convert.ToDateTime(reader["Timestamp"]),
                    IP = reader["IP"].ToString()
                });

                totalCount = Convert.ToInt32(reader["TotalCount"]);
            }

            return (logs, totalCount);
        }

        public async Task<List<AuditLogDto>> GetAuditLogsByUser(Guid userId)
        {
            List<AuditLogDto> logs = new();

            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_GetAuditLogsByUser", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                logs.Add(new AuditLogDto
                {
                    Id = Guid.Parse(reader["Id"].ToString()!),
                    UserId = Guid.Parse(reader["UserId"].ToString()!),
                    Action = reader["Action"].ToString()!,
                    Timestamp = Convert.ToDateTime(reader["Timestamp"]),
                    IP = reader["IP"].ToString()
                });
            }

            return logs;
        }
    }
}
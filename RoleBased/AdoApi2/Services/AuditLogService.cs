using AdoApi2.Repositories.Interfaces;
using PocoClasses.Dto;

namespace AdoApi2.Services
{
    public class AuditLogService(IAuditLogRepository repo)
    {
        public async Task InsertAuditLog(AuditLogDto dto)
        {
            await repo.InsertAuditLog(dto);
        }

        public async Task<(List<AuditLogDto> Logs, int TotalCount)>
            GetAuditLogsPaged(int page, int pageSize)
        {
            return await repo.GetAuditLogsPaged(page, pageSize);
        }

        public async Task<List<AuditLogDto>> GetAuditLogsByUser(Guid userId)
        {
            return await repo.GetAuditLogsByUser(userId);
        }
    }
}
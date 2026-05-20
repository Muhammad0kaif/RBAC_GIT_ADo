using PocoClasses.Dto;

namespace AdoApi2.Repositories.Interfaces
{
    public interface IAuditLogRepository
    {
        Task InsertAuditLog(AuditLogDto dto);

        Task<(List<AuditLogDto> Logs, int TotalCount)>
            GetAuditLogsPaged(int page, int pageSize);

        Task<List<AuditLogDto>> GetAuditLogsByUser(Guid userId);
    }
}

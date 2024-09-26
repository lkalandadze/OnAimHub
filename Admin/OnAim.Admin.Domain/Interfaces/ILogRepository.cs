using OnAim.Admin.Domain.Entities;

namespace OnAim.Admin.Domain.Interfaces;

public interface ILogRepository
{
    Task AddAuditLogAsync(AuditLog auditLog);
    Task AddRejectedLogAsync(RejectedLog rejectedLog);
    Task<List<AuditLog>> GetUserLogs(int userId);
}

using OnAim.Admin.Infrasturcture.Entities;

namespace OnAim.Admin.APP.Services.Abstract
{
    public interface IAuditLogService
    {
        Task LogEventAsync(AuditLog audit);
        Task RetryRejectedLogsAsync();
    }
}

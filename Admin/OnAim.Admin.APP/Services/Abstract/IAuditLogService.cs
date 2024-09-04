namespace OnAim.Admin.APP.Services.Abstract
{
    public interface IAuditLogService
    {
        Task LogEventAsync(DateTimeOffset timestamp, string actionType, string entityType, int entityId, int userId, string description);
        Task RetryRejectedLogsAsync();
    }
}

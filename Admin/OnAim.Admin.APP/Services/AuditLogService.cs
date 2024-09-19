using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

public class AuditLogService : IAuditLogService
{
    private readonly IConfigurationRepository<AuditLog> _auditLogRepository;
    private readonly IRejectedLogRepository _rejectedLogRepository;

    public AuditLogService(IConfigurationRepository<AuditLog> auditLogRepository, IRejectedLogRepository rejectedLogRepository)
    {
        _auditLogRepository = auditLogRepository;
        _rejectedLogRepository = rejectedLogRepository;
    }

    public async Task LogEventAsync(AuditLog audit)
    {
        try
        {
            var auditLog = new AuditLog
            {
                Timestamp = audit.Timestamp,
                Action = audit.Action,
                ObjectId = audit.ObjectId,
                Log = audit.Log,
                UserId = audit.UserId,
            };
            await _auditLogRepository.Store(auditLog);
            await _auditLogRepository.CommitChanges();

            await _auditLogRepository.UnitOfWork.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            await HandleRejectedLog(audit, ex.Message);
        }
    }

    private async Task HandleRejectedLog(AuditLog auditLog, string errorMessage)
    {
        var rejectedLog = new RejectedLog
        {
            Action = auditLog.Action,
            ObjectId = auditLog.ObjectId,
            Log = auditLog.Log,
            UserId = auditLog.UserId,
            Timestamp = auditLog.Timestamp,
            ErrorMessage = errorMessage
        };

        await _rejectedLogRepository.AddAsync(rejectedLog);
    }

    public async Task RetryRejectedLogsAsync()
    {
        var rejectedLogs = await _rejectedLogRepository.GetPendingLogsAsync();

        foreach (var log in rejectedLogs)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    UserId = log.UserId,
                };
                await _auditLogRepository.Store(auditLog);
                await _auditLogRepository.CommitChanges();

                await _rejectedLogRepository.MarkAsProcessedAsync(log.Id);
            }
            catch
            {
                await _rejectedLogRepository.IncrementRetryCountAsync(log.Id);
            }
        }
    }
}
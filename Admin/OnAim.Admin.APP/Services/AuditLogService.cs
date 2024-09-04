using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IConfigurationRepository<Infrasturcture.Entities.AuditLog> _auditLogRepository;
        private readonly IRejectedLogRepository _rejectedLogRepository;

        public AuditLogService(IConfigurationRepository<Infrasturcture.Entities.AuditLog> auditLogRepository, IRejectedLogRepository rejectedLogRepository)
        {
            _auditLogRepository = auditLogRepository;
            _rejectedLogRepository = rejectedLogRepository;
        }

        public async Task LogEventAsync(DateTimeOffset timestamp, string actionType, string entityType, int entityId, int userId, string description)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    Timestamp = timestamp,
                    ActionType = actionType,
                    EntityType = entityType,
                    EntityId = entityId,
                    UserId = userId,
                    Description = description
                };
                await _auditLogRepository.Store(auditLog);
                await _auditLogRepository.CommitChanges();
            }
            catch (Exception ex)
            {
                await HandleRejectedLog(actionType, entityType, entityId, userId, description, ex.Message);
            }
        }

        private async Task HandleRejectedLog(string actionType, string entityType, int entityId, int userId, string description, string errorMessage)
        {
            var rejectedLog = new RejectedLog
            {
                ActionType = actionType,
                EntityType = entityType,
                EntityId = entityId,
                UserId = userId,
                Description = description,
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
                        ActionType = log.ActionType,
                        EntityType = log.EntityType,
                        EntityId = log.EntityId,
                        UserId = log.UserId,
                        Description = log.Description
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
}

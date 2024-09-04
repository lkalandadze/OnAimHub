using Microsoft.Extensions.Hosting;
using OnAim.Admin.APP.Services.Abstract;

namespace OnAim.Admin.APP.Services
{
    public class RejectedLogRetryService : BackgroundService
    {
        private readonly IAuditLogService _auditLogService;

        public RejectedLogRetryService(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _auditLogService.RetryRejectedLogsAsync();

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}

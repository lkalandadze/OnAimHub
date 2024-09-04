using OnAim.Admin.Infrasturcture.Entities;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IRejectedLogRepository
    {
        Task AddAsync(RejectedLog rejectedLog);
        Task<List<RejectedLog>> GetPendingLogsAsync();
        Task MarkAsProcessedAsync(int id);
        Task IncrementRetryCountAsync(int id);
    }
}

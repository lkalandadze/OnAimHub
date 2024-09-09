using Hub.Domain.Entities;

namespace Hub.Application.Services.Abstract.BackgroundJobs;

public interface IBackgroundJobScheduler
{
    void ScheduleJob(Job job, string cronExpression);
    void RemoveScheduledJob(int jobId);
    void ExecuteJob(string currencyId);
}
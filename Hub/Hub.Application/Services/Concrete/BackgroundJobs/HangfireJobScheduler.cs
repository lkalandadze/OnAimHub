using Hangfire;
using Hub.Application.Services.Abstract;
using Hub.Application.Services.Abstract.BackgroundJobs;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Absractions;
using Hub.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Hub.Application.Services.Concrete.BackgroundJobs;

public class HangfireJobScheduler : IBackgroundJobScheduler
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HangfireJobScheduler(IRecurringJobManager recurringJobManager, IServiceScopeFactory serviceScopeFactory)
    {
        _recurringJobManager = recurringJobManager;
        _serviceScopeFactory = serviceScopeFactory;
    }
    public void ScheduleJob(Job job, string cronExpression)
    {
        RecurringJob.AddOrUpdate(
            job.Id.ToString(),
            () => ExecuteJob(job.CurrencyId),
            cronExpression,
            TimeZoneInfo.Local);
    }


    public void RemoveScheduledJob(int jobId)
    {
        RecurringJob.RemoveIfExists(jobId.ToString());
    }

    public void ExecuteJob(string currencyId)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
            var playerBalanceService = scope.ServiceProvider.GetRequiredService<IPlayerBalanceService>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var job = jobRepository.Query().FirstOrDefault(j => j.CurrencyId == currencyId);

            if (job != null)
            {
                playerBalanceService.ResetBalancesByCurrencyIdAsync(currencyId).GetAwaiter().GetResult();

                job.LastExecutedTime = DateTime.UtcNow;

                jobRepository.Update(job);
                unitOfWork.SaveAsync().GetAwaiter().GetResult();
            }
        }
    }

    private string GenerateCronExpression(TimeSpan executionTime, int intervalInDays)
    {
        int hour = executionTime.Hours;
        int minute = executionTime.Minutes;

        return $"0 {minute} {hour} */{intervalInDays} * ?";
    }
}

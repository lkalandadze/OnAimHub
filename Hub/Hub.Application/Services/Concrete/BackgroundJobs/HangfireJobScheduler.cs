using Hangfire;
using Hub.Application.Services.Abstract;
using Hub.Application.Services.Abstract.BackgroundJobs;
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
    public void ScheduleJob(Job job)
    {
        RecurringJob.AddOrUpdate(
            job.Id.ToString(),
            () => ExecuteJob(job.CurrencyId),
            job.CronExpression,
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
            var playerBalanceService = scope.ServiceProvider.GetRequiredService<IPlayerBalanceService>();
            playerBalanceService.ResetBalancesByCurrencyIdAsync(currencyId).GetAwaiter().GetResult();
        }
    }
}

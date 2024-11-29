using Hangfire;
using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;
using Microsoft.Extensions.DependencyInjection;

namespace Leaderboard.Application.Services.Concrete.BackgroundJobs;

public class BackgroundJobScheduler : IBackgroundJobScheduler
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BackgroundJobScheduler(IRecurringJobManager recurringJobManager, IServiceScopeFactory serviceScopeFactory)
    {
        _recurringJobManager = recurringJobManager;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void ScheduleJob(LeaderboardSchedule schedule)
    {
        if (schedule.RepeatType == RepeatType.None)
            return; // Skip scheduling for SingleDate as it doesn't repeat.

        var cronExpression = GenerateCronExpression(schedule);

        _recurringJobManager.AddOrUpdate(
            schedule.Id.ToString(),
            () => ExecuteJob(schedule.Id),
            cronExpression,
            TimeZoneInfo.Local
        );

        // Schedule periodic status updates
        _recurringJobManager.AddOrUpdate(
            "UpdateLeaderboardStatuses",
            () => UpdateLeaderboardStatuses(),
            Cron.Hourly(),
            TimeZoneInfo.Local
        );
    }

    //public void ScheduleRecordJob(LeaderboardRecord job)
    //{
    //    var cronExpression = GenerateCronExpression(job.JobType); // Automatically generate cron expression
    //    _recurringJobManager.AddOrUpdate(
    //        job.Id.ToString(),
    //        () => ExecuteRecordJob(job.Id), // Use .Value if it's not null
    //        cronExpression,
    //        TimeZoneInfo.Local
    //    );
    //}

    public void RemoveScheduledJob(int jobId)
    {
        RecurringJob.RemoveIfExists(jobId.ToString());
    }

    public void ExecuteJob(int scheduleId)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
            jobService.ExecuteLeaderboardRecordGeneration(scheduleId).GetAwaiter().GetResult();
        }
    }

    public void UpdateLeaderboardStatuses()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
            jobService.UpdateLeaderboardRecordStatusAsync().GetAwaiter().GetResult();
        }
    }

    // Make sure this method is public and generates the cron expression
    public string GenerateCronExpression(LeaderboardSchedule schedule)
    {
        return schedule.RepeatType switch
        {
            RepeatType.EveryNDays => Cron.DayInterval(schedule.RepeatValue ?? 1),
            RepeatType.DayOfWeek => Cron.Weekly((DayOfWeek)(schedule.RepeatValue ?? 0)),
            RepeatType.DayOfMonth => Cron.Monthly(schedule.RepeatValue ?? 1),
            _ => throw new ArgumentException("Unsupported repeat type.")
        };
    }
}
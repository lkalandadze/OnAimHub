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
        // Generate a cron expression based on the schedule details
        var cronExpression = GenerateCronExpression(schedule);

        _recurringJobManager.AddOrUpdate(
            schedule.Id.ToString(),
            () => ExecuteJob(schedule.Id), // Job for the LeaderboardSchedule
            cronExpression,
            TimeZoneInfo.Local
        );

        // Schedule a recurring job to update leaderboard statuses every hour
        _recurringJobManager.AddOrUpdate(
            "UpdateLeaderboardStatuses",
            () => UpdateLeaderboardStatuses(),
            Cron.Hourly(), // This job runs hourly to check for status updates
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

    public void ExecuteJob(int leaderboardTemplateId)
    {
        // Create a new scope for each job execution
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            try
            {
                var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();

                // Execute the leaderboard record generation logic
                jobService.ExecuteLeaderboardRecordGeneration(leaderboardTemplateId).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                Console.WriteLine($"Error executing job for template ID {leaderboardTemplateId}: {ex.Message}");
                throw;
            }
        }
    }

public void ExecuteRecordJob(int id)
{
    // Create a new scope for each job execution
    using (var scope = _serviceScopeFactory.CreateScope())
    {
        try
        {
            var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();

            // Execute the leaderboard record generation logic
            jobService.ExecuteLeaderboardJob(id).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            // Log the error (optional)
            Console.WriteLine($"Error executing job for record ID {id}: {ex.Message}");
            throw;
        }
    }
}

    public void UpdateLeaderboardStatuses()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            try
            {
                var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
                jobService.UpdateLeaderboardRecordStatusAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating leaderboard record statuses: {ex.Message}");
                throw;
            }
        }
    }

    // Make sure this method is public and generates the cron expression
    public string GenerateCronExpression(LeaderboardSchedule schedule)
    {
        return schedule.RepeatType switch
        {
            RepeatType.SingleDate => Cron.Yearly(schedule.StartDate.Month, schedule.StartDate.Day), // Run once on the specified date
            RepeatType.EveryNDays => Cron.DayInterval(schedule.RepeatValue ?? 1), // Run every N days
            RepeatType.DayOfWeek => Cron.Weekly((DayOfWeek)(schedule.RepeatValue ?? 0)), // Run weekly on specified day
            RepeatType.DayOfMonth => Cron.Monthly(schedule.RepeatValue ?? 1), // Run monthly on specified day
            _ => throw new ArgumentException("Unsupported repeat type")
        };
    }
}
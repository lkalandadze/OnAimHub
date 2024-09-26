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

    public void ScheduleJob(LeaderboardRecord job)
    {
        var cronExpression = GenerateCronExpression(job.JobType); // Automatically generate cron expression
        _recurringJobManager.AddOrUpdate(
            job.Id.ToString(),
            () => ExecuteJob(job.LeaderboardTemplateId.Value), // Use .Value if it's not null
            cronExpression,
            TimeZoneInfo.Local
        );
    }

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

    // Make sure this method is public and generates the cron expression
    public string GenerateCronExpression(JobTypeEnum jobType)
    {
        return jobType switch
        {
            JobTypeEnum.Daily => Cron.Daily(),
            JobTypeEnum.Weekly => Cron.Weekly(DayOfWeek.Monday),
            JobTypeEnum.Monthly => Cron.Monthly(1),
            JobTypeEnum.Custom => Cron.Hourly(), // Adjust for custom logic
            _ => throw new ArgumentException("Unsupported job type"),
        };
    }
}
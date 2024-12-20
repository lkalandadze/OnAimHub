﻿using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Leaderboard.Application.Services.Concrete.BackgroundJobs;

public class JobSyncService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public JobSyncService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
                    var backgroundJobScheduler = scope.ServiceProvider.GetRequiredService<IBackgroundJobScheduler>();

                    // Retrieve all active schedules
                    var schedules = await jobService.GetAllActiveSchedulesAsync();

                    foreach (var schedule in schedules)
                    {
                        // Schedule each active leaderboard schedule
                        backgroundJobScheduler.ScheduleJob(schedule);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception, handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Sync every 5 minutes
        }
    }
}
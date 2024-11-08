using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Leaderboard.Application.Services.Concrete.BackgroundJobs;

public class LeaderboardScheduleStatusUpdaterService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public LeaderboardScheduleStatusUpdaterService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();

                // Update the status of completed schedules
                await jobService.UpdateScheduleStatusesAsync();
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Run every 5 minutes
        }
    }
}
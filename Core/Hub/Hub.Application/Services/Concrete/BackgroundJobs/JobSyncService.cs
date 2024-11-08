using Hub.Application.Services.Abstract.BackgroundJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hub.Application.Services.Concrete.BackgroundJobs;

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
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();

                await jobService.SyncJobsWithHangfireAsync();
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
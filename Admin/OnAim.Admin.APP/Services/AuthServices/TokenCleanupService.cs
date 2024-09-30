using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnAim.Admin.Infrasturcture.Persistance.Data;

namespace OnAim.Admin.APP.Services.AuthServices;

public class TokenCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TokenCleanupService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var expiredAccessTokens = context.AccessTokens
                    .Where(at => at.Expiration <= DateTime.UtcNow);

                context.AccessTokens.RemoveRange(expiredAccessTokens);

                var expiredRefreshTokens = context.RefreshTokens
                    .Where(rt => rt.Expiration <= DateTime.UtcNow && !rt.IsRevoked);

                context.RefreshTokens.RemoveRange(expiredRefreshTokens);

                await context.SaveChangesAsync(stoppingToken);
            }
        }
    }
}

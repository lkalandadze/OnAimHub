using Leaderboard.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Leaderboard.Infrastructure;

public class DbInitializer
{
    public static async Task InitializeDatabase(IServiceProvider serviceProvider, LeaderboardDbContext context)
    {

        #region Migrations
        using IServiceScope serviceScope = serviceProvider.CreateScope();

        try
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            var initializer = new DbInitializer();
            await initializer.Seed(serviceProvider, context);
        }
        catch (Exception ex)
        {
            var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<LeaderboardDbContext>>();



            logger.LogError(ex, "An error occurred while migrating or seeding the database.");



            throw;
        }
        #endregion
    }

    #region Seeding

    private async Task Seed(IServiceProvider serviceProvider, LeaderboardDbContext context)
    {
        using var scope = serviceProvider.CreateScope();
        context.Database.EnsureCreated();
        await SeedLeaderboardTemplate(context, scope);
    }

    private static async Task SeedLeaderboardTemplate(LeaderboardDbContext context, IServiceScope scope)
    {
    }

    #endregion
}
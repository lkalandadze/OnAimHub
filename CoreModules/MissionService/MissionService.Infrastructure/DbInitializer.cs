using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MissionService.Infrastructure.DataAccess;

namespace MissionService.Infrastructure;

public class DbInitializer
{
    public static async Task InitializeDatabase(IServiceProvider serviceProvider, MissionDbContext context)
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
            var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<MissionDbContext>>();



            logger.LogError(ex, "An error occurred while migrating or seeding the database.");



            throw;
        }
        #endregion
    }

    #region Seeding

    private async Task Seed(IServiceProvider serviceProvider, MissionDbContext context)
    {
        using var scope = serviceProvider.CreateScope();
        context.Database.EnsureCreated();
        await SeedMission(context, scope);
    }

    private static async Task SeedMission(MissionDbContext context, IServiceScope scope)
    {
    }

    #endregion
}
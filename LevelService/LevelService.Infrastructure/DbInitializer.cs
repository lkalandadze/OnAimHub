using LevelService.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LevelService.Infrastructure;

public class DbInitializer
{
    public static async Task InitializeDatabase(IServiceProvider serviceProvider, LevelDbContext context)
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
            var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<LevelDbContext>>();



            logger.LogError(ex, "An error occurred while migrating or seeding the database.");



            throw;
        }
        #endregion
    }

    #region Seeding

    private async Task Seed(IServiceProvider serviceProvider, LevelDbContext context)
    {
        using var scope = serviceProvider.CreateScope();
        context.Database.EnsureCreated();
        await SeedLevel(context, scope);
    }

    private static async Task SeedLevel(LevelDbContext context, IServiceScope scope)
    {
    }

    #endregion
}
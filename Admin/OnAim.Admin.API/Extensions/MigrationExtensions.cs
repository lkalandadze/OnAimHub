using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;

namespace OnAim.Admin.API.Extensions;

public static class MigrationExtensions
{
    public async static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        ILogger logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("MigrationExtensions");
        DatabaseContext dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        try
        {
            var connectionString = dbContext.Database.GetDbConnection().ConnectionString;
            logger.LogInformation("Starting database migration. Connection String: {ConnectionString}", connectionString);

            dbContext.Database.Migrate();
            logger.LogInformation("Database migration completed.");

            await dbContext.SeedDatabaseAsync();
            logger.LogInformation("Database seeding completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying migrations.");
        }
    }
}

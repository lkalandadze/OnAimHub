using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Persistance.Data;

namespace OnAim.Admin.API.Extensions;

public static class MigrationExtensions
{
    public async static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using DatabaseContext dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        dbContext.Database.Migrate();
        await dbContext.SeedDatabaseAsync();
    }
}

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OnAim.Admin.Infrasturcture.Persistance.Data;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args = null)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<DatabaseContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");

        builder.UseNpgsql(connectionString);

        return new DatabaseContext(builder.Options);
    }
}

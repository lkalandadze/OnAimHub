using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;
using System.Reflection;

namespace GameLib.Infrastructure;

public class DbInitializer : BaseDbInitializer
{
    public DbInitializer(IServiceScope serviceScope)
    {
        var dbContext = serviceScope.ServiceProvider.GetService<SharedGameConfigDbContext>();

        if (dbContext == null)
        {
            return;
        }

        SeedDbEnums(dbContext, Assembly.GetAssembly(typeof(Price))!).Wait();
    }
}
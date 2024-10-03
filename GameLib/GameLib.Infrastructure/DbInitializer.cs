using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;
using Shared.Infrastructure;
using System.Reflection;

namespace GameLib.Infrastructure;

public class DbInitializer : BaseDbInitializer
{
    public DbInitializer(SharedGameConfigDbContext dbContext)
    {
        if (dbContext == null)
        {
            return;
        }

        SeedDbEnums(dbContext, Assembly.GetAssembly(typeof(Price))!).Wait();
    }
}
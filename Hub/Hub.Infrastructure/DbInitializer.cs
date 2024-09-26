﻿using Hub.Domain.Entities;
using Hub.Domain.Enum;
using Hub.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;
using System.Reflection;
namespace Hub.Infrastructure;

public class DbInitializer : BaseDbInitializer
{
    public DbInitializer(IServiceScope serviceScope)
    {
        var dbContext = serviceScope.ServiceProvider.GetService<HubDbContext>();

        if (dbContext == null)
        {
            return;
        }

        SeedDbEnums(dbContext, Assembly.GetAssembly(typeof(Player))!).Wait();
        SeedJobs(dbContext).Wait();
        SeedDefaultSegment(dbContext).Wait();
        SeedGames(dbContext).Wait();
    }

    // temporary ?
    protected static async Task SeedGames(HubDbContext dbContext)
    {
        if (!dbContext.Games.Any())
        {
            var game = new Game("Wheel");

            await dbContext.Games.AddAsync(game);
            await dbContext.SaveChangesAsync();
        }
    }

    protected static async Task SeedDefaultSegment(HubDbContext dbContext)
    {
        if (!dbContext.Segments.Any())
        {
            var segment = new Segment("default", string.Empty, 1);

            await dbContext.Segments.AddAsync(segment);
            await dbContext.SaveChangesAsync();
        }
    }

    protected static async Task SeedJobs(HubDbContext dbContext)
    {
        if (!dbContext.Jobs.Any(x => x.Name == "Reset Daily Progress"))
        {
            var job = new Job("Reset Daily Progress", "Clear player progress table daily", "OnAimCoin", true, null, null, JobType.Daily);

            await dbContext.Jobs.AddAsync(job);
            await dbContext.SaveChangesAsync();
        }
    }
}
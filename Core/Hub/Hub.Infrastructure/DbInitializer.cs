﻿using Hub.Domain.Entities;
using Hub.Domain.Enum;
using Hub.Infrastructure.DataAccess;
using Shared.Infrastructure;
using System.Reflection;

namespace Hub.Infrastructure;

public class DbInitializer : BaseDbInitializer
{
    public DbInitializer(HubDbContext dbContext)
    {
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
            var games = new List<Game>
            {
                new Game("Wheel"),
                new Game("PenaltyKicks"),
            };

            foreach (var game in games)
            {
                await dbContext.Games.AddAsync(game);
            }

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
        if (!dbContext.Jobs.Any(j => j.Category == JobCategory.DailyProgressReset))
        {
            var job = new Job("Daily Progress Reset", "Clears player progresses table daily.", true, JobType.Daily, JobCategory.DailyProgressReset);

            await dbContext.Jobs.AddAsync(job);
            await dbContext.SaveChangesAsync();
        }
    }
}
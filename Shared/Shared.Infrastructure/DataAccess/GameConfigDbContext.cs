using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;
using System.Reflection;

namespace Shared.Infrastructure.DataAccess;

public class GameConfigDbContext() : DbContext
{
    public DbSet<Prize> Prizes { get; set; }
    public DbSet<PrizeType> PrizeTypes { get; set; }
    public DbSet<PrizeGroup> PrizeGroups { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Currency> Curencies { get; set; }
    public DbSet<Segment> Segments { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<GameVersion> GameVersions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(GameConfigDbContext)));

        base.OnModelCreating(modelBuilder);
    }
}
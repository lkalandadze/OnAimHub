using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;
using System.Reflection;

namespace Shared.Infrastructure.DataAccess;
public class SharedGameConfigDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<GameVersion> GameVersions { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<Currency> Curencies { get; set; }
    public DbSet<Segment> Segments { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<PrizeType> PrizeTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(SharedGameConfigDbContext))!);

        base.OnModelCreating(modelBuilder);
    }
}
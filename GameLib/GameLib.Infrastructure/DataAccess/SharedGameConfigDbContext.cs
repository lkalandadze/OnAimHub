using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GameLib.Domain.Entities;

namespace GameLib.Infrastructure.DataAccess;

public abstract class SharedGameConfigDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Segment> Segments { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<PrizeType> PrizeTypes { get; set; }
    public DbSet<GameSetting> GameSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(SharedGameConfigDbContext))!);

        base.OnModelCreating(modelBuilder);
    }
}
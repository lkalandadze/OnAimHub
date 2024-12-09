using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GameLib.Domain.Entities;

namespace GameLib.Infrastructure.DataAccess;

public abstract class SharedGameConfigDbContext(DbContextOptions options) : DbContext(options)
{

}

public abstract class SharedGameConfigDbContext<T> : SharedGameConfigDbContext where T : GameConfiguration<T>
{
    public SharedGameConfigDbContext(DbContextOptions<SharedGameConfigDbContext> options)
        : base(options)
    {

    }

    public DbSet<Coin> Coins { get; set; }
    public DbSet<T> GameConfigurations { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<PrizeType> PrizeTypes { get; set; }
    public DbSet<GameSetting> GameSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(SharedGameConfigDbContext<>))!);

        base.OnModelCreating(modelBuilder);
    }
}
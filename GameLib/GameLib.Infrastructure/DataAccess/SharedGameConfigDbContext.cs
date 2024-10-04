using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GameLib.Domain.Entities;

namespace GameLib.Infrastructure.DataAccess;

public abstract class SharedGameConfigDbContext(DbContextOptions options) : DbContext(options)
{

}

public abstract class SharedGameConfigDbContext<T>(DbContextOptions options) : SharedGameConfigDbContext(options) where T : GameConfiguration<T>
{
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<T> Segments { get; set; }
    public DbSet<T> GameConfigurations { get; set; }
    public DbSet<T> Prices { get; set; }
    public DbSet<PrizeType> PrizeTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(SharedGameConfigDbContext<>))!);

        base.OnModelCreating(modelBuilder);
    }
}
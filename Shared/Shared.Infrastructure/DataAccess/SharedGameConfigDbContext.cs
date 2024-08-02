using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using Shared.Domain.Entities;
using Shared.Infrastructure.TypeConfigurations;
using System.Reflection;

namespace Shared.Infrastructure.DataAccess;
public class SharedGameConfigDbContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<Base.Prize> Prizes { get; set; }
    public DbSet<Base.PrizeType> PrizeTypes { get; set; }
    public DbSet<Base.PrizeGroup> PrizeGroups { get; set; }
    public DbSet<Base.Price> Prices { get; set; }
    public DbSet<Base.Currency> Curencies { get; set; }
    public DbSet<Base.Segment> Segments { get; set; }
    public DbSet<Base.Configuration> Configurations { get; set; }
    public DbSet<Base.GameVersion> GameVersions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(PrizeGroupTypeConfiguration))!);
        modelBuilder.Entity<Base.Prize>().ToTable(TableNames.Prizes);

        base.OnModelCreating(modelBuilder);
    }
}
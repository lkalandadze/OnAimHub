using Microsoft.EntityFrameworkCore;

namespace Test;

public class GameDbContext(DbContextOptions options) : BaseDbContext(options)
{
    public DbSet<WheelPrizeGroup> WheelPrizeGroups { get; set; }
    public DbSet<WheelPrizeGroup> JackpotPrizeGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WheelPrizeGroup>()
            .ToTable("WheelPrizeGroups");

        modelBuilder.Entity<WheelPrizeGroup>()
            .ToTable("JackpotPrizeGroups");
    }
}
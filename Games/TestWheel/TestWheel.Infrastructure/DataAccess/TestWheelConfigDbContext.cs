using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using TestWheel.Domain.Entities;

namespace TestWheel.Infrastructure.DataAccess;

public class TestWheelConfigDbContext : SharedGameConfigDbContext<TestWheelConfiguration>
{
    //public WheelConfigDbContext(DbContextOptions<SharedGameConfigDbContext<WheelConfiguration>> options)
    //    : base(options)
    //{
    //}

    public TestWheelConfigDbContext(DbContextOptions<SharedGameConfigDbContext> options)
        : base(options)
    {
    }

    public DbSet<TestWheelPrize> TestWheelPrizes { get; set; }
    public DbSet<JackpotPrizeGroup> JackpotPrizeGroups { get; set; }
    public DbSet<JackpotPrize> JackpotPrizes { get; set; }
    public DbSet<Round> Rounds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
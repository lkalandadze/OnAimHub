#nullable disable

using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Wheel.Domain.Entities;

namespace Wheel.Infrastructure.DataAccess;

public class WheelConfigDbContext(DbContextOptions<SharedGameConfigDbContext> options) 
    : SharedGameConfigDbContext<WheelConfiguration>(options)
{
    public DbSet<WheelPrize> WheelPrizes { get; set; }
    public DbSet<Round> Rounds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.DataAccess;
using Wheel.Domain.Entities;
using Shared.Domain;

namespace Wheel.Infrastructure.DataAccess;

public class WheelConfigDbContext(DbContextOptions<WheelConfigDbContext> options) : SharedGameConfigDbContext(options)
{
    public DbSet<Prize> Prizes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Prize>().ToTable(TableNames.Prizes);

        base.OnModelCreating(modelBuilder);
    }
}
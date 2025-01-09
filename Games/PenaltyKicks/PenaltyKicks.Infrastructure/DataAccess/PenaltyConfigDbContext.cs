#nullable disable

using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using PenaltyKicks.Domain.Entities;

namespace PenaltyKicks.Infrastructure.DataAccess;

public class PenaltyConfigDbContext(DbContextOptions<SharedGameConfigDbContext> options)
    : SharedGameConfigDbContext<PenaltyConfiguration>(options)
{
    public DbSet<PenaltyGame> PenaltyGame { get; set; }
    public DbSet<PenaltyPrize> PenaltyPrizes { get; set; }
    public DbSet<PenaltySeries> PenaltySeries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
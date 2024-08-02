using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;

namespace Shared.Infrastructure.DataAccess;

public class SharedGameHistoryDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Base.PrizeHistory> PrizeHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
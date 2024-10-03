using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.LeaderBoradEntities;

namespace OnAim.Admin.Infrasturcture.Persistance.Data.Hub;

public class ReadOnlyDataContext : DbContext
{
    public ReadOnlyDataContext(DbContextOptions<ReadOnlyDataContext> options)
        : base(options)
    {
    }

    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerBalance> PlayerBalances { get; set; }
    public DbSet<PlayerProgress> PlayerProgresses { get; set; }
    public DbSet<PlayerProgressHistory> PlayerProgressHistories { get; set; }
    public DbSet<Segment> Segments { get; set; }
    public DbSet<PlayerSegment> PlayerSegments { get; set; }
    public DbSet<PlayerBlockedSegment> PlayerBlockedSegments { get; set; }
    public DbSet<PlayerSegmentAct> PlayerSegmentActs { get; set; }
    public DbSet<PlayerSegmentActHistory> PlayerSegmentActHistories { get; set; }
    public DbSet<PlayerSegmentActType> PlayerSegmentActTypes { get; set; }
    public DbSet<PlayerLog> PlayerLogs { get; set; }
    public DbSet<PlayerLogType> PlayerLogTypes { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionStatus> TransactionStatuses { get; set; }
    public DbSet<TransactionType> TransactionTypes { get; set; }
    public DbSet<AccountType> AccountTypes { get; set; }
    public DbSet<ReferralDistribution> ReferralDistributions { get; set; }
    public DbSet<PlayerBan> PlayerBans { get; set; }
    //public DbSet<Currency> Currencies { get; set; }

    public IQueryable<TEntity> Set<TEntity>() where TEntity : class
    {
        return base.Set<TEntity>().AsNoTracking();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerSegment>().Ignore(e => e.Id);

        modelBuilder.Entity<PlayerSegment>().HasKey(ps => new { ps.PlayerId, ps.SegmentId });

        modelBuilder.Entity<PlayerSegment>().HasOne(ps => ps.Player)
               .WithMany(p => p.PlayerSegments)
               .HasForeignKey(ps => ps.PlayerId);

        modelBuilder.Entity<PlayerSegment>().HasOne(ps => ps.Segment)
               .WithMany(s => s.PlayerSegments)
               .HasForeignKey(ps => ps.SegmentId);

        modelBuilder.Entity<PlayerBlockedSegment>().Ignore(e => e.Id);

        modelBuilder.Entity<PlayerBlockedSegment>().HasKey(ps => new { ps.PlayerId, ps.SegmentId });

        modelBuilder.Entity<PlayerBlockedSegment>().HasOne(ps => ps.Player)
               .WithMany(p => p.PlayerBlockedSegments)
               .HasForeignKey(ps => ps.PlayerId);

        modelBuilder.Entity<PlayerBlockedSegment>().HasOne(ps => ps.Segment)
               .WithMany(s => s.PlayerBlockedSegments)
               .HasForeignKey(ps => ps.SegmentId);

        base.OnModelCreating(modelBuilder);
    }
}

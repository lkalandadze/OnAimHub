using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Hub.Infrastructure.DataAccess;

public class HubDbContext(DbContextOptions options) : DbContext(options)
{
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
    public DbSet<TokenRecord> TokenRecords { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<ConsulLog> ConsulLogs { get; set; }
    public DbSet<ReferralDistribution> ReferralDistributions { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<PlayerBan> PlayerBans { get; set; }
    public DbSet<Reward> Rewards { get; set; }
    public DbSet<RewardPrize> RewardPrizes { get; set; }
    public DbSet<PrizeType> PrizeTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(HubDbContext))!);

        base.OnModelCreating(modelBuilder);
    }
}
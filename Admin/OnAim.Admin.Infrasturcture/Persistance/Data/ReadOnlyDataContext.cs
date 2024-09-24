using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.HubEntities;

namespace OnAim.Admin.Infrasturcture.Persistance.Data
{
    public class ReadOnlyDataContext : DbContext
    {
        public ReadOnlyDataContext(DbContextOptions<ReadOnlyDataContext> options)
            : base(options)
        {
        }

        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<ConsulLog> ConsulLogs { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerBalance> PlayerBalances { get; set; }
        public DbSet<PlayerBan> PlayerBans { get; set; }
        public DbSet<PlayerLog> PlayerLogs { get; set; }
        public DbSet<PlayerLogType> PlayerLogTypes { get; set; }
        public DbSet<PlayerProgress> PlayerProgresses { get; set; }
        public DbSet<PlayerProgressHistory> PlayerProgressHistories { get; set; }
        public DbSet<PlayerSegment> PlayerSegments { get; set; }
        public DbSet<ReferralDistribution> ReferralDistributions { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<TokenRecord> TokenRecords { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionStatus> TransactionStatuses { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }

        public IQueryable<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>().AsNoTracking();
        }
    }
}

using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;
using Hub.Domain.Entities.Coins;
using Hub.Domain.Entities.Templates;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hub.Infrastructure.DataAccess;

public class HubDbContext(DbContextOptions options) : DbContext(options)
{
    private IDbContextTransaction _currentTransaction;

    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerBalance> PlayerBalances { get; set; }
    public DbSet<PlayerProgress> PlayerProgresses { get; set; }
    public DbSet<PlayerProgressHistory> PlayerProgressHistories { get; set; }
    public DbSet<Segment> Segments { get; set; }
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
    public DbSet<HubSetting> HubSettings { get; set; }
    public DbSet<PlayerBan> PlayerBans { get; set; }
    public DbSet<Reward> Rewards { get; set; }
    public DbSet<RewardPrize> RewardPrizes { get; set; }
    public DbSet<PrizeType> PrizeTypes { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    //public DbSet<PromotionService> PromotionServices { get; set; }
    public DbSet<PromotionView> PromotionViews { get; set; }
    public DbSet<Coin> Coins { get; set; }
    public DbSet<WithdrawOption> WithdrawOptions { get; set; }
    public DbSet<WithdrawOptionEndpoint> WithdrawOptionEndpoints { get; set; }
    public DbSet<WithdrawOptionGroup> WithdrawOptionGroups { get; set; }
    public DbSet<Service> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(HubDbContext))!);

        base.OnModelCreating(modelBuilder);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _currentTransaction ??= await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            if (_currentTransaction != null) await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransaction()
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
            }
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return Database.CreateExecutionStrategy();
    }
}
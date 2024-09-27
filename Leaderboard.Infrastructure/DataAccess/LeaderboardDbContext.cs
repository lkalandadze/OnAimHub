using Leaderboard.Domain.Entities;
using Leaderboard.Infrastructure.EntityConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Leaderboard.Infrastructure.DataAccess;

public class LeaderboardDbContext : DbContext
{
    private IDbContextTransaction _currentTransaction;

    public LeaderboardDbContext(DbContextOptions<LeaderboardDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(LeaderboardTemplateConfiguration).Assembly);
    }

    public DbSet<LeaderboardTemplate> LeaderboardTemplate { get; set; }
    public DbSet<LeaderboardRecord> LeaderboardRecords { get; set; }
    public DbSet<LeaderboardRecordPrize> LeaderboardRecordPrizes { get; set; }
    public DbSet<LeaderboardTemplatePrize> LeaderboardTemplatePrize { get; set; }
    public DbSet<Currency> Currencies { get; set; }

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

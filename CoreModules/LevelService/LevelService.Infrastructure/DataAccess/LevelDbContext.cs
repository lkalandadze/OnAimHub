using LevelService.Domain.Entities;
using LevelService.Domain.Entities.DbEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LevelService.Infrastructure.DataAccess;

public class LevelDbContext : DbContext
{
    private IDbContextTransaction _currentTransaction;

    public LevelDbContext(DbContextOptions<LevelDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }

    public DbSet<Stage> Stages { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<LevelPrize> LevelPrizes { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<PrizeType> PrizeTypes { get; set; }

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

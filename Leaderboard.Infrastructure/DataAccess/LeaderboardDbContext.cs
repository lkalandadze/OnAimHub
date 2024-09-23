using Leaderboard.Domain.Entities;
using Leaderboard.Infrastructure.EntityConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Leaderboard.Infrastructure.DataAccess;

public class LeaderboardDbContext : DbContext
{
    protected readonly IConfiguration Configuration;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    private IDbContextTransaction _currentTransaction;

    public LeaderboardDbContext(DbContextOptions<LeaderboardDbContext> options, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        Configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(LeaderboardTemplateConfiguration).Assembly);
    }

    public DbSet<LeaderboardTemplate> LeaderboardTemplate { get; set; }

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

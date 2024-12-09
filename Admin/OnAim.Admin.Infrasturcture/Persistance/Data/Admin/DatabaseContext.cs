using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Interfaces;
using System.Data;

namespace OnAim.Admin.Infrasturcture.Persistance.Data.Admin;

public class DatabaseContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _dbContextTransaction;

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<EndpointGroup> EndpointGroups { get; set; } = null!;
    public DbSet<Endpoint> Endpoints { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<RoleEndpointGroup> RoleEndpointGroups { get; set; } = null!;
    public DbSet<EndpointGroupEndpoint> EndpointGroupEndpoints { get; set; } = null!;
    public DbSet<AllowedEmailDomain> AllowedDomains { get; set; } = null!;
    public DbSet<AppSetting> AppSettings { get; set; } = null!;
    public DbSet<AccessToken> AccessTokens { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }

    public async Task<IDisposable> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
    {
        _dbContextTransaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        return _dbContextTransaction;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_dbContextTransaction != null)
        {
            await _dbContextTransaction.CommitAsync(cancellationToken);
            await _dbContextTransaction.DisposeAsync();
            _dbContextTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_dbContextTransaction != null)
        {
            await _dbContextTransaction.RollbackAsync(cancellationToken);
            await _dbContextTransaction.DisposeAsync();
            _dbContextTransaction = null;
        }
    }
}

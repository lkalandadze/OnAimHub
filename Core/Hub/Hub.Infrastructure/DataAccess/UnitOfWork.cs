using Hub.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hub.Infrastructure.DataAccess;

public class UnitOfWork(HubDbContext context) : IUnitOfWork, IDisposable
{
    private readonly HubDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private IDbContextTransaction? _currentTransaction;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("There is already an active transaction.");
        }

        _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("There is no active transaction to commit.");
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("There is no active transaction to rollback.");
        }

        try
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    //public async Task SaveAsync(CancellationToken cancellationToken = default)
    //{
    //    if (_currentTransaction == null)
    //    {
    //        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

    //        try
    //        {
    //            await _context.SaveChangesAsync(cancellationToken);
    //            await transaction.CommitAsync(cancellationToken);
    //        }
    //        catch
    //        {
    //            await transaction.RollbackAsync(cancellationToken);
    //            throw;
    //        }
    //    }
    //    else
    //    {
    //        await _context.SaveChangesAsync(cancellationToken);
    //    }
    //}

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction == null)
            {
                // If no transaction is present, just save the changes without beginning a new transaction
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                // If there's an active transaction, just save the changes without starting a new transaction
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            if (_currentTransaction != null)
            {
                await RollbackTransactionAsync(cancellationToken); // Ensure rollback happens if there is a failure during SaveChangesAsync
            }
            throw;
        }
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
    }
}
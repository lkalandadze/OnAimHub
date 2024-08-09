using Hub.Domain.Absractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hub.Infrastructure.DataAccess;

public class UnitOfWork(HubDbContext context) : IUnitOfWork, IDisposable
{
    private readonly HubDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private IDbContextTransaction? _currentTransaction;

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("There is already an active transaction.");
        }

        _currentTransaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("There is no active transaction to commit.");
        }

        try
        {
            await _context.SaveChangesAsync();
            await _currentTransaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("There is no active transaction to rollback.");
        }

        try
        {
            await _currentTransaction.RollbackAsync();
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task SaveAsync()
    {
        if (_currentTransaction == null)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        else
        {
            await _context.SaveChangesAsync();
        }
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _context.Dispose();
    }
}
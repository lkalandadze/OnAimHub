using GameLib.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace GameLib.Infrastructure.DataAccess;

public class UnitOfWork(SharedGameConfigDbContext context) : IUnitOfWork, IDisposable
{
    private readonly SharedGameConfigDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
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
        try
        {
            if (_currentTransaction == null)
            {
                // If no transaction is present, just save the changes without beginning a new transaction
                await _context.SaveChangesAsync();
            }
            else
            {
                // If there's an active transaction, just save the changes without starting a new transaction
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (_currentTransaction != null)
            {
                await RollbackTransactionAsync(); // Ensure rollback happens if there is a failure during SaveChangesAsync
            }
            throw;
        }
    }

    public void Dispose()
    {
        if (_currentTransaction != null)
        {
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }
}
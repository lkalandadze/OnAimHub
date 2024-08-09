namespace Hub.Domain.Absractions;

public interface IUnitOfWork
{
    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();

    Task SaveAsync();
}
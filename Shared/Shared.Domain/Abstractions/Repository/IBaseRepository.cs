using System.Linq.Expressions;

namespace Shared.Domain.Abstractions.Repository;

public interface IBaseRepository<TAggregateRoot> where TAggregateRoot : BaseEntity
{
    Task<TAggregateRoot?> OfIdAsync(int id);

    IQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>>? expression = default);

    Task<List<TAggregateRoot>> QueryAsync(Expression<Func<TAggregateRoot, bool>>? expression = default);

    void Delete(TAggregateRoot aggregateRoot);

    Task InsertAsync(TAggregateRoot aggregateRoot);

    void Update(TAggregateRoot aggregateRoot);

    Task SaveAsync();
}

public interface IBaseRepository
{
    Task<BaseEntity?> OfIdAsync(int id);

    IQueryable<BaseEntity> Query(Expression<Func<BaseEntity, bool>>? expression = default);

    Task<List<BaseEntity>> QueryAsync(Expression<Func<BaseEntity, bool>>? expression = default);

    void Delete(BaseEntity aggregateRoot);

    Task InsertAsync(BaseEntity aggregateRoot);

    void Update(BaseEntity aggregateRoot);

    Task SaveAsync();
}
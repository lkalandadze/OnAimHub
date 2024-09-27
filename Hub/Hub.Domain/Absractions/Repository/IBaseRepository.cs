using Shared.Domain.Entities;
using System.Linq.Expressions;

namespace Hub.Domain.Absractions.Repository;

public interface IBaseRepository<TAggregateRoot> 
    where TAggregateRoot : BaseEntity
{
    Task<TAggregateRoot?> OfIdAsync(int id);

    IQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>>? expression = default);

    Task<List<TAggregateRoot>> QueryAsync(Expression<Func<TAggregateRoot, bool>>? expression = default);

    void Delete(TAggregateRoot aggregateRoot);

    Task InsertAsync(TAggregateRoot aggregateRoot);

    Task InsertRangeAsync(IEnumerable<TAggregateRoot> aggregateRoots);

    void Update(TAggregateRoot aggregateRoot);
}
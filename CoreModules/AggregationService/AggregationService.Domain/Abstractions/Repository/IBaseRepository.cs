using System.Linq.Expressions;

namespace AggregationService.Domain.Abstractions.Repository;

public interface IBaseRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity, Expression<Func<T, bool>> predicate);
    Task DeleteAsync(Expression<Func<T, bool>> predicate);
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>>? predicate = null);
    Task<long> CountAsync(Expression<Func<T, bool>>? predicate = null);
    IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null);
    Task CommitChangesAsync();
}
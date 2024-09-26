using System.Linq.Expressions;

namespace OnAim.Admin.Domain.Interfaces;

public interface IReadOnlyRepository<T> where T : class
{
    IUnitOfWork UnitOfWork { get; }

    Task<T?> Find(int uId);

    IQueryable<T> Query(Expression<Func<T, bool>>? expression = null);
}

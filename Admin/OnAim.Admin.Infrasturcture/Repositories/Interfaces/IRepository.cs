using OnAim.Admin.Infrasturcture.Interfaces;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;
using System.Linq.Expressions;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract;

public interface IRepository<T> where T : class
{
    IUnitOfWork UnitOfWork { get; }

    Task<T?> Find(int uId);

    IQueryable<T> Query(Expression<Func<T, bool>>? expression = null);

    Task<IQueryable<T>> QueryAsync(Expression<Func<T, bool>>? expression = null);

    Task Store(T document);

    void Remove(T document);

    void WithDbContext(DatabaseContext dbContext);

    Task CommitChanges();
}

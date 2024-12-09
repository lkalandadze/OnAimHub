using OnAim.Admin.Infrasturcture.Interfaces;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;
using System.Linq.Expressions;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract;

public interface IConfigurationRepository<T> where T : class
{
    IUnitOfWork UnitOfWork { get; }

    Task<T?> Find(Expression<Func<T, bool>> expression);

    IQueryable<T> Query(Expression<Func<T, bool>>? expression = null);

    Task Store(T document);

    Task Remove(T document);

    void WithDbContext(DatabaseContext dbContext);

    Task CommitChanges();
}

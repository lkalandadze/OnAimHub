using OnAim.Admin.Infrasturcture.Interfaces;
using System.Linq.Expressions;

namespace OnAim.Admin.Infrasturcture.Repositories.Abstract;

public interface IPromotionRepository<T> where T : class
{
    IUnitOfWork UnitOfWork { get; }

    Task<T?> Find(int uId);

    IQueryable<T> Query(Expression<Func<T, bool>>? expression = null);
}
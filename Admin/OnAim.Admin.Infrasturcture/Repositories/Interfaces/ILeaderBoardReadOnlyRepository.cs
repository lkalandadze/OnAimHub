using System.Linq.Expressions;

namespace OnAim.Admin.Infrasturcture.Interfaces;

public interface ILeaderBoardReadOnlyRepository<T> where T : class
{
    IUnitOfWork UnitOfWork { get; }

    Task<T?> Find(int uId);

    IQueryable<T> Query(Expression<Func<T, bool>>? expression = null);
}

using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture.Persistance.Data.LeaderBoard;
using System.Linq.Expressions;

namespace OnAim.Admin.Infrasturcture.Repositories.Leaderboard;

public class LeaderBoardReadOnlyRepository<T> : ILeaderBoardReadOnlyRepository<T> where T : class
{
    private readonly LeaderBoardReadOnlyDataContext _context;

    public LeaderBoardReadOnlyRepository(LeaderBoardReadOnlyDataContext context)
    {
        _context = context;
    }
    public IUnitOfWork UnitOfWork => throw new NotImplementedException();

    public async Task<T?> Find(int id)
    {
        var entities = await _context.Set<T>().ToListAsync();
        return entities.SingleOrDefault(x => (int)x.GetType().GetProperty("Id")!.GetValue(x) == id);
    }

    public IQueryable<T> Query(Expression<Func<T, bool>>? expression = null)
    {
        var baseQuery = _context.Set<T>();

        if (expression == null)
            return baseQuery.AsQueryable();
        return baseQuery.Where(expression).AsQueryable();
    }
}

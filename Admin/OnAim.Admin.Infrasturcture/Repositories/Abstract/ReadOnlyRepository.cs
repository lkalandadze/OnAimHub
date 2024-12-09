using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Interfaces;
using OnAim.Admin.Infrasturcture.Persistance.Data.Hub;
using System.Linq.Expressions;

namespace OnAim.Admin.Infrasturcture.Repositories.Abstract;

public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
{
    private readonly ReadOnlyDataContext _db;

    public IUnitOfWork UnitOfWork => throw new NotImplementedException();

    public ReadOnlyRepository(ReadOnlyDataContext db)
    {
        _db = db;
    }
    public async Task<T?> Find(int id)
    {
        var entities = await _db.Set<T>().ToListAsync();
        return entities.SingleOrDefault(x => (int)x.GetType().GetProperty("Id")!.GetValue(x) == id);
    }

    public IQueryable<T> Query(Expression<Func<T, bool>>? expression = null)
    {
        var baseQuery = _db.Set<T>();

        if (expression == null)
            return baseQuery.AsQueryable();
        return baseQuery.Where(expression).AsQueryable();
    }
}

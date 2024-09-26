using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using System.Linq.Expressions;

namespace OnAim.Admin.Infrasturcture.Repository;

public class ConfigurationRepository<T> : IConfigurationRepository<T> where T : class
{
    private DatabaseContext _configurationDbContext;

    public ConfigurationRepository(DatabaseContext configurationDbContext)
    {
        _configurationDbContext = configurationDbContext;
    }

    public IUnitOfWork UnitOfWork
    {
        get
        {
            return _configurationDbContext;
        }
    }

    public virtual async Task<T?> Find(Expression<Func<T, bool>> expression)
    {
        return await _configurationDbContext.Set<T>().SingleOrDefaultAsync(expression);
    }

    public virtual IQueryable<T> Query(
        Expression<Func<T, bool>> expression = null
       )
    {
        var baseQuery = _configurationDbContext.Set<T>();

        if (expression == null)
            return baseQuery.AsQueryable();
        return baseQuery.Where(expression).AsQueryable();
    }

    public virtual async Task Store(T document)
    {
        await _configurationDbContext.Set<T>().AddAsync(document);
    }


    public virtual async Task Remove(T document)
    {
        _configurationDbContext.Set<T>().Remove(document);
        await Task.CompletedTask;
    }

    public void WithDbContext(DatabaseContext dbContext)
    {
        _configurationDbContext = dbContext;
    }

    public async Task CommitChanges()
    {
        await _configurationDbContext.SaveChangesAsync();
    }
}

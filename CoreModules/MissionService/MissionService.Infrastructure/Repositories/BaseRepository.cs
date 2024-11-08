using Microsoft.EntityFrameworkCore;
using MissionService.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using System.Linq.Expressions;

namespace MissionService.Infrastructure.Repositories;

public class BaseRepository<TContext, TAggregateRoot>(TContext context) : IBaseRepository<TAggregateRoot>
    where TAggregateRoot : BaseEntity
    where TContext : DbContext
{
    protected readonly TContext _context = context;

    public async Task<TAggregateRoot?> OfIdAsync(int id)
    {
        return await _context.Set<TAggregateRoot>().FindAsync(id);
    }

    public IQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>>? expression = default)
    {
        return expression == null ? _context.Set<TAggregateRoot>().AsQueryable() : _context.Set<TAggregateRoot>().Where(expression);
    }

    public async Task<List<TAggregateRoot>> QueryAsync(Expression<Func<TAggregateRoot, bool>>? expression = default)
    {
        if (expression == null)
        {
            return await _context.Set<TAggregateRoot>().ToListAsync();
        }
        else
        {
            return await _context.Set<TAggregateRoot>().Where(expression).ToListAsync();
        }
    }

    public virtual async Task InsertAsync(TAggregateRoot aggregateRoot)
    {
        await _context.Set<TAggregateRoot>().AddAsync(aggregateRoot);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual void Update(TAggregateRoot aggregateRoot)
    {
        _context.Entry(aggregateRoot).State = EntityState.Modified;
    }

    public virtual void Delete(TAggregateRoot aggregateRoot)
    {
        _context.Set<TAggregateRoot>().Remove(aggregateRoot);
    }
}
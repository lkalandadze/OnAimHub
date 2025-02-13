﻿using GameLib.Domain.Abstractions.Repository;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;
using System.Linq.Expressions;

namespace GameLib.Infrastructure.Repositories;

public abstract class BaseRepository<TContext, TAggregateRoot>(TContext context) : IBaseRepository<TAggregateRoot>
    where TAggregateRoot : BaseEntity
    where TContext : DbContext
{
    protected readonly TContext _context = context;

    public virtual async Task<TAggregateRoot?> OfIdAsync(dynamic id)
    {
        return await _context.Set<TAggregateRoot>().FindAsync(id);
    }

    public virtual IQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>>? expression = default)
    {
        return expression == null ? _context.Set<TAggregateRoot>().AsQueryable() : _context.Set<TAggregateRoot>().Where(expression);
    }

    public virtual async Task<List<TAggregateRoot>> QueryAsync(Expression<Func<TAggregateRoot, bool>>? expression = default)
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

    public virtual void Update(TAggregateRoot aggregateRoot)
    {
        _context.Entry(aggregateRoot).State = EntityState.Modified;
    }

    public virtual void Delete(TAggregateRoot aggregateRoot)
    {
        _context.Set<TAggregateRoot>().Remove(aggregateRoot);
    }

    public async Task SaveAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
﻿using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Infrasturcture.Interfaces;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using System.Linq.Expressions;

namespace OnAim.Admin.Infrasturcture.Repositories.Abstract;

public class EfRepository<T> : IRepository<T>
    where T : BaseEntity
{
    private DatabaseContext _db;


    public EfRepository(DatabaseContext db)
    {
        _db = db;
    }

    public IUnitOfWork UnitOfWork
    {
        get
        {
            return _db;
        }
    }

    public virtual async Task<T?> Find(int uId)
    {
        return await _db.Set<T>().SingleOrDefaultAsync(x => x.Id == uId);
    }

    public virtual IQueryable<T> Query(
        Expression<Func<T, bool>> expression = null
        )
    {
        var baseQuery = _db.Set<T>();

        if (expression == null)
            return baseQuery.AsQueryable();
        return baseQuery.Where(expression).AsQueryable();
    }

    public virtual async Task Store(T document)
    {
        await _db.Set<T>().AddAsync(document);
    }

    public void WithDbContext(DatabaseContext dbContext)
    {
        _db = dbContext;
    }

    public async Task CommitChanges()
    {
        await _db.SaveChangesAsync();
    }

    public async void Remove(T document)
    {
        _db.Set<T>().Remove(document);
        await Task.CompletedTask;
    }

    public async Task<IQueryable<T>> QueryAsync(Expression<Func<T, bool>>? expression = null)
    {
        var query = expression == null ? _db.Set<T>() : _db.Set<T>().Where(expression);
        return await Task.FromResult(query);
    }
}

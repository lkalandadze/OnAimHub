﻿using OnAim.Admin.Infrasturcture.Persistance.Data;
using System.Linq.Expressions;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IConfigurationRepository<T> where T : class
    {
        Task<T?> Find(Expression<Func<T, bool>> expression);

        IQueryable<T> Query(Expression<Func<T, bool>>? expression = null);

        Task Store(T document);

        Task Remove(T document);

        void WithDbContext(DatabaseContext dbContext);

        Task CommitChanges();
    }
}

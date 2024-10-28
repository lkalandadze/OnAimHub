using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace GameLib.Infrastructure.Repositories;

public class GameConfigurationRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, GameConfiguration>(context), IGameConfigurationRepository
{
    public override async Task<GameConfiguration?> OfIdAsync(dynamic id)
    {
        int convertedId = Convert.ToInt32(id);

        return await IncludeNavigationProperties(GetDbSet()).Where(c => c.Id == convertedId)
                                                            .FirstOrDefaultAsync();
    }

    public override IQueryable<GameConfiguration> Query(Expression<Func<GameConfiguration, bool>>? expression = null)
    {
        return expression != null ? GetDbSet().Where(expression) : GetDbSet();
    }

    public override async Task<List<GameConfiguration>> QueryAsync(Expression<Func<GameConfiguration, bool>>? expression = null)
    {
        return expression != null ? GetDbSet().Where(expression).ToList() : await GetDbSet().ToListAsync();
    }

    public override void Delete(GameConfiguration aggregateRoot)
    {
        base.Delete(aggregateRoot);
    }

    public void InsertConfigurationTree(GameConfiguration aggregateRoot)
    {
        var dbSet = GetDbSet();

        var addMethod = dbSet.GetType().GetMethod(nameof(DbSet<object>.Add));

        if (addMethod == null)
        {
            throw new ArgumentNullException(nameof(addMethod));
        }

        var parameters = new object[] { aggregateRoot };

        addMethod.Invoke(dbSet, parameters);
    }

    public async Task UpdateConfigurationTreeAsync(GameConfiguration updatedEntity)
    {
        var dbSet = GetDbSet();

        var existingEntity = await IncludeNavigationProperties(dbSet)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == updatedEntity.Id);

        if (existingEntity == null)
        {
            throw new KeyNotFoundException($"Entity with ID {updatedEntity.Id} not found.");
        }

        var properties = GetGameConfigurationType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (IsCollection(property.PropertyType))
            {
                var existingCollection = property.GetValue(existingEntity) as IEnumerable;
                var updatedCollection = property.GetValue(updatedEntity) as IEnumerable;

                if (existingCollection != null && updatedCollection != null)
                {
                    UpdateCollection(existingCollection, updatedCollection, property.PropertyType);
                }
            }
            else
            {
                var updatedValue = property.GetValue(updatedEntity);

                if (updatedValue != null)
                {
                    property.SetValue(existingEntity, updatedValue);
                }
            }
        }

        base.Update(existingEntity);
    }

    public void UpdateCollection(IEnumerable existingCollection, IEnumerable updatedCollection, Type collectionType)
    {
        if (existingCollection == null || updatedCollection == null)
        {
            return;
        }

        var itemType = collectionType.GetGenericArguments().FirstOrDefault();

        if (itemType == null)
        {
            return;
        }

        var clearMethod = typeof(ICollection<>).MakeGenericType(itemType).GetMethod(nameof(ICollection<object>.Clear));
        clearMethod?.Invoke(existingCollection, null);

        var addMethod = typeof(ICollection<>).MakeGenericType(itemType).GetMethod(nameof(ICollection<object>.Add));

        foreach (var updatedItem in updatedCollection)
        {
            var entityEntry = _context.Entry(updatedItem);

            if (entityEntry.State == EntityState.Detached)
            {
                _context.Attach(updatedItem);
            }

            entityEntry.State = EntityState.Modified;

            addMethod?.Invoke(existingCollection, [updatedItem]);
        }
    }

    public bool IsCollection(Type type)
    {
        return type.IsGenericType && (typeof(ICollection<>).IsAssignableFrom(type.GetGenericTypeDefinition()) || typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()));
    }

    private Type GetGameConfigurationType()
    {
        var dbSets = context.GetType()
                            .GetProperties()
                            .Where(p => p.PropertyType.IsGenericType 
                                     && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

        return dbSets.Select(dbSet => dbSet.PropertyType.GetGenericArguments()[0])
                                                        .First(setType => typeof(GameConfiguration)
                                                        .IsAssignableFrom(setType));
    }

    private IQueryable<GameConfiguration> GetDbSet()
    {
        var configType = GetGameConfigurationType();

        var setMethod = typeof(DbContext)
            .GetMethods()
            .First(m => m.Name == nameof(DbContext.Set) && m.IsGenericMethod);

        var genericSetMethod = setMethod.MakeGenericMethod(configType);
        var dbSet = genericSetMethod.Invoke(_context, null);

        if (dbSet == null)
        {
            throw new ArgumentNullException(nameof(dbSet));
        }

        return (IQueryable<GameConfiguration>)dbSet;
    }

    private IQueryable<GameConfiguration> IncludeNavigationProperties(IQueryable<GameConfiguration> query)
    {
        var derivedEntityTypes = context.Model.GetEntityTypes()
            .Where(e => typeof(GameConfiguration).IsAssignableFrom(e.ClrType) && !e.ClrType.IsAbstract);

        foreach (var entityType in derivedEntityTypes)
        {
            var navigationProperties = entityType.GetNavigations().Select(n => n.Name).Distinct();

            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }
        }

        return query;
    }

    //#pragma warning disable CS0809

    //[Obsolete("This method is obsolete and should not be used. Please use InsertConfigurationTree(GameConfiguration aggregateRoot) instead.", true)]
    //public override Task InsertAsync(GameConfiguration aggregateRoot)
    //{
    //    throw new NotImplementedException();
    //}

    //[Obsolete("This method is obsolete and should not be used. Please use UpdateConfigurationTreeAsync(GameConfiguration updatedEntity) instead.", true)]
    //public override void Update(GameConfiguration aggregateRoot)
    //{
    //    throw new NotImplementedException();
    //}

    //#pragma warning restore CS0809
}
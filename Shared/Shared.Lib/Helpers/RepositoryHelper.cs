using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Shared.Lib.Helpers;

public static class RepositoryHelper
{
    public static Type GetDbSetEntityType<T>(DbContext context) where T : class
    {
        var dbSets = context.GetType()
                            .GetProperties()
                            .Where(p => p.PropertyType.IsGenericType
                                     && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

        return dbSets.Select(dbSet => dbSet.PropertyType.GetGenericArguments()[0])
                                                        .First(setType => typeof(T)
                                                        .IsAssignableFrom(setType));
    }

    public static IQueryable<T> GetDbSet<T>(DbContext context) where T : class
    {
        var configType = GetDbSetEntityType<T>(context);

        var setMethod = typeof(DbContext)
            .GetMethods()
            .First(m => m.Name == nameof(DbContext.Set) && m.IsGenericMethod);

        var genericSetMethod = setMethod.MakeGenericMethod(configType);
        var dbSet = genericSetMethod.Invoke(context, null);

        if (dbSet == null)
        {
            throw new ArgumentNullException(nameof(dbSet));
        }

        return (IQueryable<T>)dbSet;
    }

    public static IQueryable<T> IncludeNavigationProperties<T>(DbContext context, IQueryable<T> query) where T : class
    {
        var derivedEntityTypes = context.Model.GetEntityTypes()
            .Where(e => typeof(T).IsAssignableFrom(e.ClrType) && !e.ClrType.IsAbstract);

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

    public static void UpdateCollection(DbContext context, IEnumerable existingCollection, IEnumerable updatedCollection, Type collectionType)
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
            var entityEntry = context.Entry(updatedItem);

            if (entityEntry.State == EntityState.Detached)
            {
                context.Attach(updatedItem);
            }

            entityEntry.State = EntityState.Modified;

            addMethod?.Invoke(existingCollection, [updatedItem]);
        }
    }
}
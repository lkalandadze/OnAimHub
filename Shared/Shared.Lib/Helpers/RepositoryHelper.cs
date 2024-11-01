using Microsoft.EntityFrameworkCore;
using OnAim.Lib.EntityExtension.GlobalAttributes.Attributes;
using System.Collections;
using System.Reflection;

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

    public static IQueryable<TEntity> IncludeNotHiddenAll<TEntity>(this IQueryable<TEntity> query) where TEntity : class
    {
        return IncludeNotHiddenAll(query, typeof(TEntity));
    }

    public static IQueryable<TEntity> IncludeNotHiddenAll<TEntity>(this IQueryable<TEntity> query, Type type) where TEntity : class
    {
        var pathsToInclude = GetAllNavigationPaths(query, type, "", []);

        pathsToInclude.ForEach(path =>
        {
            query = query.Include(path);
        });

        return query;
    }

    public static List<string> GetAllNavigationPaths<TEntity>(IQueryable<TEntity> query, Type entityType, string path, List<string> paths) where TEntity : class
    {
        var navigations = entityType.GetProperties()
            .Where(p => IsNavigableProperty(p) && p.Name != "Id")
            .Where(p => p.GetCustomAttribute<IgnoreIncludeAllAttribute>() == null)
            .ToList();

        foreach (var navigation in navigations)
        {
            if (!TypeHelper.IsGenericCollection(navigation.PropertyType))
            {
                continue;
            }

            var navigationPath = string.IsNullOrEmpty(path) ? navigation.Name : $"{path}.{navigation.Name}";

            var isTypeHidden = navigation.PropertyType.GetGenericArguments().First()
                                         .GetCustomAttributes<IgnoreIncludeAllAttribute>().Any();

            if (!navigation.PropertyType.IsGenericType || isTypeHidden)
            {
                continue;
            }

            paths.Add(navigationPath);

            Type navigationType = navigation.PropertyType;

            navigationType = navigation.PropertyType.IsGenericType
                ? navigation.PropertyType.GetGenericArguments().FirstOrDefault()
                : navigation.PropertyType.GetElementType();

            paths = GetAllNavigationPaths(query, navigationType, navigationPath, paths);
        }

        return paths.Distinct().ToList();
    }

    public static bool IsNavigableProperty(PropertyInfo property)
    {
        var type = property.PropertyType;

        if (type.IsPrimitive || type == typeof(string) || type.IsEnum || type == typeof(DateTime) || type == typeof(decimal))
        {
            return false;
        }

        return type.IsClass || TypeHelper.IsGenericCollection(type);
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
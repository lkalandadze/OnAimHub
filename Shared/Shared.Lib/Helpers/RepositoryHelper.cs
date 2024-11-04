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

    public static void UpdateEntityTreeRecursive(DbContext context, object existingEntity, object updatedEntity)
    {
        var entityType = existingEntity.GetType();
        var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.Name == "Id")
            {
                continue;
            }
            else if (TypeHelper.IsGenericCollection(property.PropertyType))
            {
                var existingCollection = property.GetValue(existingEntity) as IEnumerable<object>;
                var updatedCollection = property.GetValue(updatedEntity) as IEnumerable<object>;

                if (existingCollection != null && updatedCollection != null)
                {
                    UpdateCollection(context, existingCollection, updatedCollection);
                }
            }
            else
            {
                property.SetValue(existingEntity, property.GetValue(updatedEntity));
            }
        }
    }

    public static void UpdateCollection(DbContext context, IEnumerable<object> oldCollection, IEnumerable<object> newCollection)
    {
        //Remove
        var removedItems = oldCollection.Where(x => newCollection.All(xx => !EntityHelper.HaveSameId(x, xx))).ToList();
        removedItems.ForEach(r => CollectionHelper.RemoveFromCollection(oldCollection, r));

        //Add
        var added = newCollection.Where(x =>
        {
            var id = EntityHelper.GetId(x);
            return (id is int intId && intId == 0) || (id is string strId && string.IsNullOrEmpty(strId));
        }).ToList();

        added.ForEach(a => CollectionHelper.AddToCollection(oldCollection, a));

        //Update
        var updatedInNew = newCollection.Where(x => !EntityHelper.IsNewEntity(x)).ToList();
        var updateInOld = oldCollection.Where(x => updatedInNew.Any(xx => EntityHelper.HaveSameId(x, xx))).ToList();

        foreach (var newItem in updatedInNew)
        {
            var existingItem = updateInOld.First(x => EntityHelper.HaveSameId(x, newItem));
            UpdateEntityTreeRecursive(context, existingItem, newItem);

            context.Entry(existingItem).State = EntityState.Modified;
        }
    }
}
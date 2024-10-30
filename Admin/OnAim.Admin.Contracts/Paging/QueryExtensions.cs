using OnAim.Admin.Contracts.Dtos.Base;
using System.Linq.Expressions;
using System.Reflection;

namespace OnAim.Admin.Contracts.Paging;

public static class QueryExtensions
{
    public static IQueryable<TEntity> ApplyFilters<TEntity, TFilter>(
        IQueryable<TEntity> query,
        TFilter filter,
        CancellationToken cancellationToken) where TEntity : class where TFilter : BaseFilter
    {
        var filterProperties = typeof(TFilter).GetProperties();

        foreach (var property in filterProperties)
        {
            var value = property.GetValue(filter);
            if (value is null) continue;

            if (property.PropertyType == typeof(bool?))
            {
                if (value is bool boolValue)
                {
                    query = query.Where(CreatePredicate<TEntity>(property.Name, boolValue));
                }
            }
            else if (value is IEnumerable<object> enumerable && enumerable.Cast<object>().Any())
            {
                query = query.Where(CreateCollectionPredicate<TEntity>(property.Name, enumerable));
            }
            else if (property.Name.EndsWith("From") || property.Name.EndsWith("To"))
            {
                query = ApplyDateFilters(query, property, value);
            }
        }

        return query;
    }

    private static Expression<Func<TEntity, bool>> CreatePredicate<TEntity>(string propertyName, bool value)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var property = Expression.Property(parameter, propertyName);
        var constant = Expression.Constant(value);

        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.Equal(property, constant),
            parameter);
    }

    private static Expression<Func<TEntity, bool>> CreateCollectionPredicate<TEntity>(string propertyName, IEnumerable<object> values)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var property = Expression.Property(parameter, propertyName);
        var constant = Expression.Constant(values);

        var containsMethod = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
            .MakeGenericMethod(property.Type);

        var containsCall = Expression.Call(containsMethod, constant, property);

        return Expression.Lambda<Func<TEntity, bool>>(containsCall, parameter);
    }

    private static IQueryable<TEntity> ApplyDateFilters<TEntity>(IQueryable<TEntity> query, PropertyInfo property, object value)
    {
        var isFrom = property.Name.EndsWith("From");
        var isTo = property.Name.EndsWith("To");

        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var propertyAccess = Expression.Property(parameter, property.Name);
        var constant = Expression.Constant(value);

        if (isFrom)
        {
            return query.Where(Expression.Lambda<Func<TEntity, bool>>(Expression.GreaterThanOrEqual(propertyAccess, constant), parameter));
        }
        else if (isTo)
        {
            return query.Where(Expression.Lambda<Func<TEntity, bool>>(Expression.LessThanOrEqual(propertyAccess, constant), parameter));
        }

        return query;
    }
}

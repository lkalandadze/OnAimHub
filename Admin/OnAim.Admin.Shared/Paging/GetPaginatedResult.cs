using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Shared.DTOs.Base;
using System.Linq.Expressions;
using System.Reflection;

namespace OnAim.Admin.Shared.Paging;

public class Paginator
{
    public static async Task<PaginatedResult<TDto>> GetPaginatedResult<TEntity, TDto, TFilter>(
        IQueryable<TEntity> query,
        TFilter filter,
        Expression<Func<TEntity, TDto>> projection,
        List<string> sortableFields)
        where TEntity : class
        where TFilter : BaseFilter
    {
        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;
        var sortDescending = filter.SortDescending.GetValueOrDefault();
        var sortBy = filter.SortBy?.ToLower();
        var sortableFieldList = sortableFields;

        Expression<Func<TEntity, object>> sortExpression = CreateSortExpression<TEntity>(sortBy);

        query = sortDescending ? query.OrderByDescending(sortExpression) : query.OrderBy(sortExpression);

        var totalCount = await query.CountAsync();

        var items = await query
            .Select(projection)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<TDto>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items,
            SortableFields = sortableFields,
        };
    }

    private static Expression<Func<TEntity, object>> CreateSortExpression<TEntity>(string sortBy)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");

        var property = typeof(TEntity).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property == null)
        {
            throw new ArgumentException($"Property '{sortBy}' is not defined for type '{typeof(TEntity).Name}'.");
        }

        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        return Expression.Lambda<Func<TEntity, object>>(Expression.Convert(propertyAccess, typeof(object)), parameter);
    }
}

using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Contracts.Dtos.Base;
using System.Linq.Expressions;

namespace OnAim.Admin.Contracts.Paging;

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
}

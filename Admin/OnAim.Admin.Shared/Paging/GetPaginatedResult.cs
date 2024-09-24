using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Shared.DTOs.Base;
using System.Linq.Expressions;


namespace OnAim.Admin.Shared.Paging
{
    public class Paginator
    {
        public static async Task<PaginatedResult<TDto>> GetPaginatedResult<TEntity, TDto, TFilter>(
            IQueryable<TEntity> query,
            TFilter filter,
            Expression<Func<TEntity, TDto>> projection,
            CancellationToken cancellationToken)
            where TEntity : class
            where TFilter : BaseFilter
        {
            var pageNumber = filter.PageNumber ?? 1;
            var pageSize = filter.PageSize ?? 25;
            var sortDescending = filter.SortDescending.GetValueOrDefault();
            var sortBy = filter.SortBy?.ToLower();

            Expression<Func<TEntity, object>> sortExpression = CreateSortExpression<TEntity>(sortBy);

            query = sortDescending ? query.OrderByDescending(sortExpression) : query.OrderBy(sortExpression);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Select(projection)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<TDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }

        private static Expression<Func<TEntity, object>> CreateSortExpression<TEntity>(string sortBy)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            Expression property = null;

            switch (sortBy)
            {
                case "id":
                    property = Expression.Property(parameter, "Id");
                    break;
                case "firstname":
                    property = Expression.Property(parameter, "FirstName");
                    break;
                case "lastname":
                    property = Expression.Property(parameter, "LastName");
                    break;
                case "name":
                    property = Expression.Property(parameter, "Name");
                    break;
                default:
                    property = Expression.Property(parameter, "Id");
                    break;
            }

            if (property == null)
            {
                throw new ArgumentException($"Property '{sortBy}' is not defined for type '{typeof(TEntity).Name}'.");
            }

            return Expression.Lambda<Func<TEntity, object>>(Expression.Convert(property, typeof(object)), parameter);
        }
    }
}

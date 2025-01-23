using Shared.Lib.Wrappers;

namespace Shared.Lib.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<TSource> Pagination<TSource>(this IEnumerable<TSource> source, PagedRequest request)
    {
        if (request == null)
        {
            return source;
        }

        request.PageSize ??= 10;
        request.PageNumber ??= 1;

        return source.Skip(request.PageSize.Value * (request.PageNumber.Value - 1)).Take(request.PageSize.Value);
    }

    public static async Task<IEnumerable<TSource>> PaginationAsync<TSource>(this IEnumerable<TSource> source, PagedRequest request)
    {
        if (request == null)
        {
            return source;
        }

        request.PageSize ??= 10;
        request.PageNumber ??= 1;

        return await Task.FromResult(source.Skip(request.PageSize.Value * (request.PageNumber.Value - 1)).Take(request.PageSize.Value));
    }
}
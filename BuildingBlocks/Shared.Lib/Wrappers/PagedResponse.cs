namespace Shared.Lib.Wrappers;

public class PagedResponse<T>
{
    public PagedResponse(IEnumerable<T>? data, int? pageNumber, int? pageSize, int totalCount)
    {
        Items = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = pageSize.HasValue && pageSize > 0
            ? (int)Math.Ceiling((double)totalCount / pageSize.Value)
            : 0;
    }

    public IEnumerable<T>? Items { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
namespace Shared.Lib.Wrappers;

public class PagedResponse<T>
{
    public PagedResponse(IEnumerable<T>? data, int? pageNumber, int? pageSize, int totalCount)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public IEnumerable<T>? Data { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public int TotalCount { get; set; }
}
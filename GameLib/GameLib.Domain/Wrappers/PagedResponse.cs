namespace GameLib.Domain.Wrappers;

public class PagedResponse<T> : Response<T>
{
    public PagedResponse(int pageNumber, int pageSize, int totalCount)
        : this(default, pageNumber, pageSize, totalCount, default) { }

    public PagedResponse(T? data, int pageNumber, int pageSize, int totalCount, string message = null)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        Data = data;
        Message = message;
        Succeeded = true;
        Error = null;
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public void AddData(T data)
    {
        Data = data;
    }
}
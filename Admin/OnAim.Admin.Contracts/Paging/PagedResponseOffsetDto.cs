﻿namespace OnAim.Admin.Contracts.Paging;

public record PaginatedResult<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public List<T> Items { get; set; }
    public List<string> SortableFields { get; set; }
}

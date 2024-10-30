﻿using OnAim.Admin.Contracts.Enums;

namespace OnAim.Admin.Contracts.Dtos.Base;

public record BaseFilter
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? SortBy { get; set; }
    public bool? SortDescending { get; set; }
    public bool? IsActive { get; set; }
    public HistoryStatus? HistoryStatus { get; set; }
}
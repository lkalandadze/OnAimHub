﻿using OnAim.Admin.Infrasturcture.Attributes;

namespace OnAim.Admin.Infrasturcture.Models.Request.User
{
    public record UserFilter(
        string? Name,
        string? Email,
        string? SortBy ,
        bool? SortDescending,
        int? PageNumber,
        [PageSize(100)]
        int? PageSize,
        string? Direction,
        List<int>? RoleIds,
        bool? IsActive
        );
}

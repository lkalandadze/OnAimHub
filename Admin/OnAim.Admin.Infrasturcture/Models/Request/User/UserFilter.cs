namespace OnAim.Admin.Infrasturcture.Models.Request.User
{
    public record UserFilter(
        string? Name,
        string? Email,
        int PageNumber,
        int PageSize,
        string? Direction,
        List<int>? RoleIds,
        bool? IsActive
        );
}

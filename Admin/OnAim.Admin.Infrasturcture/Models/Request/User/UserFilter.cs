namespace OnAim.Admin.Infrasturcture.Models.Request.User
{
    public record UserFilter(
        string? Name,
        string? Email,
        int PageNumber,
        int PageSize,
        string? Direction,
        List<string>? RoleIds,
        bool? IsActive
        );
}

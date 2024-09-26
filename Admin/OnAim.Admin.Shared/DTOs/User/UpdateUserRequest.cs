namespace OnAim.Admin.Shared.DTOs.User;

public class UpdateUserRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public bool? IsActive { get; set; }
    public List<int> RoleIds { get; set; }
}

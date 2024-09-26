using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.Shared.DTOs.User;

public class GetUserModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public List<RoleModel> Roles { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset DateUpdated { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public UserPreferences UserPreferences { get; set; }
    public UserDto? CreatedBy { get; set; }
    public List<LogDto> Logs { get; set; }
}

public class LogDto
{
    public string Action { get; set; }
    public string Log {  get; set; }
    public DateTimeOffset DateCreated { get; set; }
}

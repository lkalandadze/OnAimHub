using OnAim.Admin.Contracts.Dtos.Role;

namespace OnAim.Admin.Contracts.Dtos.User;

public class DownloadUserModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public List<RoleDto> Roles { get; set; }
}

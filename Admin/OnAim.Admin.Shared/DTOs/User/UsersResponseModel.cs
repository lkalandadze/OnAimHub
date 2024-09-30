using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.Shared.DTOs.User;

public class UsersResponseModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public bool IsActive { get; set; }
    public List<RoleResponseModel> Roles { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateUpdated { get; set; }
    public DateTimeOffset DateDeleted { get; set; }
}

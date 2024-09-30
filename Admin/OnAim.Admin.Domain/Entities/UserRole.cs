namespace OnAim.Admin.Domain.Entities;

public class UserRole
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public bool IsActive { get; set; } = true;

    public UserRole(int userId, int roleId)
    {
        UserId = userId;
        RoleId = roleId;
        IsActive = true;
    }
}

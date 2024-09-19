using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.Shared.DTOs.User
{
    public class UsersModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<RoleDto> Roles { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}

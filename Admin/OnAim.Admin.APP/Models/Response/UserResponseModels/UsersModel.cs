namespace OnAim.Admin.APP.Models.Response.User
{
    public class UsersModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<RoleDto> Roles { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
    }
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }
}

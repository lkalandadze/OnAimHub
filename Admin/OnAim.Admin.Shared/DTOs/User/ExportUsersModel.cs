namespace OnAim.Admin.Shared.DTOs.User
{
    public class ExportUsersModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Roles { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}

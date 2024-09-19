using OnAim.Admin.Shared.DTOs.EndpointGroup;

namespace OnAim.Admin.Shared.DTOs.Role
{
    public class RoleResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<EndpointGroupModel> EndpointGroupModels { get; set; }
        public List<UserDto> UsersResponseModels { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateDeleted { get; set; }
        public bool IsActive { get; set; }
        public UserDto? CreatedBy { get; set; }
    }
}

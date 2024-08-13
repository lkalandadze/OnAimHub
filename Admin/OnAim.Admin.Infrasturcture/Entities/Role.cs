using OnAim.Admin.Infrasturcture.Entities.Abstract;

namespace OnAim.Admin.Infrasturcture.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<RoleEndpointGroup> RoleEndpointGroups { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

        //who created/updated/deleted role
        public string? UserId { get; set; }
    }
}

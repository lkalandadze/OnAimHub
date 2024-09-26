using OnAim.Admin.Domain.Entities.Abstract;

namespace OnAim.Admin.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<RoleEndpointGroup> RoleEndpointGroups { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public bool IsDeleted { get; set; }
    public int? CreatedBy { get; set; }
}

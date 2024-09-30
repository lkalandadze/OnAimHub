using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<RoleEndpointGroup> RoleEndpointGroups { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public int? CreatedBy { get; set; }

    public Role(string name, string description, int? createdBy)
    {
        Name = name;
        Description = description;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        DateCreated = SystemDate.Now;
    }

    public void Update(string name, string description, bool isActive)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        DateUpdated = SystemDate.Now;
    }
}

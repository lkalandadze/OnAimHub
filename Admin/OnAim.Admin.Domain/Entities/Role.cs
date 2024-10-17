using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Domain.Entities;

public class Role : BaseEntity
{
    public Role()
    {
        
    }
    public Role(string name, string description, int? createdBy)
    {
        Name = name;
        Description = description;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        DateCreated = SystemDate.Now;
        RoleEndpointGroups = new List<RoleEndpointGroup>();
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<RoleEndpointGroup> RoleEndpointGroups { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public int? CreatedBy { get; set; }
}

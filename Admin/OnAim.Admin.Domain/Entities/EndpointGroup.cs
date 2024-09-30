using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Domain.Entities;

public class EndpointGroup : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<EndpointGroupEndpoint> EndpointGroupEndpoints { get; set; }
    public ICollection<RoleEndpointGroup> RoleEndpointGroups { get; set; }
    public bool IsDeleted { get; set; }
    public int? CreatedBy { get; set; }

    public EndpointGroup()
    {
        
    }
    public static EndpointGroup Create(string name, string description, int? createdBy, ICollection<EndpointGroupEndpoint>? endpointGroupEndpoint)
    {
        var endpointGroup = new EndpointGroup
        {
            Name = name,
            Description = description,
            IsDeleted = false,
            CreatedBy = createdBy,
            IsActive = true,
            DateCreated = SystemDate.Now,
            EndpointGroupEndpoints = endpointGroupEndpoint ?? new List<EndpointGroupEndpoint>()
        };

        return endpointGroup;
    }
}

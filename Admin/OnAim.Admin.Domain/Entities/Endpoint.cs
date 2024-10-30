using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Contracts.Models;

namespace OnAim.Admin.Domain.Entities;

public class Endpoint : BaseEntity
{
    public Endpoint()
    {
        
    }
    public Endpoint(
             string name,
             string path,
             int? createdBy,
             EndpointType? type,
             string description)
    {
        Name = name;
        Path = path;
        CreatedBy = createdBy;
        Type = type;
        Description = description;
        IsActive = true;
        IsDeleted = false;
        DateCreated = SystemDate.Now;
    }

    public string Path { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public int? CreatedBy { get; set; }
    public EndpointType? Type { get; set; }
    public string Description { get; set; }
    public List<EndpointGroupEndpoint> EndpointGroupEndpoints { get; set; }
}

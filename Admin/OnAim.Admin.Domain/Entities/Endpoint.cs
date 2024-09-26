using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Domain.Entities;

public class Endpoint : BaseEntity
{
    public string Path { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public int? CreatedBy { get; set; }
    public EndpointType? Type { get; set; }
    public string Description { get; set; }
    public List<EndpointGroupEndpoint> EndpointGroupEndpoints { get; set; }
}

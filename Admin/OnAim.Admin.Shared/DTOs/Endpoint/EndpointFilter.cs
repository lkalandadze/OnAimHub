using OnAim.Admin.Shared.DTOs.Base;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Shared.DTOs.Endpoint
{
    public record EndpointFilter(
        string? Name, 
        bool? IsActive, 
        EndpointType? Type, 
        List<int>? EndpointGroupIds, 
        bool? IsDeleted,
        bool? SortDescending
        ) : BaseFilter;
}

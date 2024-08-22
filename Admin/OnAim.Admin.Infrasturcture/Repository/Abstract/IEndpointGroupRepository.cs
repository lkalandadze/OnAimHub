using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup;
using OnAim.Admin.Infrasturcture.Models.Response;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IEndpointGroupRepository
    {
        Task SaveChangesAsync();
        Task DeleteAsync(int id);
        Task<EndpointGroup> GetByIdAsync(int id);
        Task AddAsync(CreateEndpointGroupRequest endpointGroup);
        Task AddEndpoint(EndpointGroup endpointGroup, Endpoint endpoint);
        Task UpdateAsync(int id, UpdateEndpointGroupRequest endpointGroup);
        Task RemoveEndpoint(EndpointGroup endpointGroup, Endpoint endpoint);
        Task<PaginatedResult<EndpointGroup>> GetAllAsync(EndpointGroupFilter filter);
    }
}

using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Infrasturcture.Models.Response;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IEndpointGroupRepository
    {
        Task SaveChangesAsync();
        Task DeleteAsync(int id);
        Task AddEndpoint(EndpointGroup endpointGroup, Endpoint endpoint);
        Task RemoveEndpoint(EndpointGroup endpointGroup, Endpoint endpoint);
        Task AddAsync(EndpointGroup endpointGroup);
        Task<EndpointGroup> GetByIdAsync(int id);
        Task<PaginatedResult<EndpointGroup>> GetAllAsync(EndpointFilter filter);
        Task UpdateAsync(EndpointGroup endpointGroup);
        Task<EndpointGroup> GetEndpointGroupByRole(int roleId);
    }
}

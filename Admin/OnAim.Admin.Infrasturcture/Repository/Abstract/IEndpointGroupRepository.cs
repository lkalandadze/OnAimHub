using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup;
using OnAim.Admin.Infrasturcture.Models.Response;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IEndpointGroupRepository
    {
        Task<EndpointGroup> GetByIdAsync(int id);
        Task AddAsync(CreateEndpointGroupRequest endpointGroup);
        Task UpdateAsync(int id, UpdateEndpointGroupRequest endpointGroup);
        Task<PaginatedResult<EndpointGroup>> GetAllAsync(EndpointGroupFilter filter);
    }
}

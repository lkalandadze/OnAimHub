using OnAim.Admin.Infrasturcture.Entities;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IEndpointGroupRepository
    {
        Task SaveChangesAsync();
        Task DeleteAsync(string id);
        Task AddEndpoint(EndpointGroup endpointGroup, Endpoint endpoint);
        Task RemoveEndpoint(EndpointGroup endpointGroup, Endpoint endpoint);
        Task AddAsync(EndpointGroup endpointGroup);
        Task<EndpointGroup> GetByIdAsync(string id);
        Task<IEnumerable<EndpointGroup>> GetAllAsync();
        Task UpdateAsync(EndpointGroup endpointGroup);
        Task<EndpointGroup> GetEndpointGroupByRole(string roleId);
    }
}

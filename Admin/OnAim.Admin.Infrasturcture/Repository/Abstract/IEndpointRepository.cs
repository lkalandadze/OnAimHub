using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Infrasturcture.Models.Response.Endpoint;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IEndpointRepository
    {
        Task CheckEndpointHealth();
        Task<List<EndpointResponseModel>> GetAllEndpoints(RoleFilter roleFilter);
        Task<EndpointResponseModel> GetEndpointById(string id);
        Task<bool> EnableEndpointAsync(string endpointId);
        Task<bool> DisableEndpointAsync(string endpointId);
        Task<IEnumerable<Endpoint>> GetEndpointsByIdsAsync(IEnumerable<string> ids);
        Task<Endpoint> CreateEndpointAsync(string path, string description = null, string? endpointType = null, string? userId = null);
    }
}

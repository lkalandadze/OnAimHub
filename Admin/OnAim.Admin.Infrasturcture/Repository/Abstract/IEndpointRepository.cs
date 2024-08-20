using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.Endpoint;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IEndpointRepository
    {
        Task CheckEndpointHealth();
        Task<PaginatedResult<EndpointResponseModel>> GetAllEndpoints(EndpointFilter roleFilter);
        Task<Endpoint> GetEndpointById(int id);
        Task<bool> EnableEndpointAsync(int endpointId);
        Task<bool> DisableEndpointAsync(int endpointId);
        Task<IEnumerable<EndpointResponseModel>> GetEndpointsByIdsAsync(IEnumerable<int> ids);
        Task<Endpoint> CreateEndpointAsync(string path, string description = null, string? endpointType = null, int? userId = null);
    }
}

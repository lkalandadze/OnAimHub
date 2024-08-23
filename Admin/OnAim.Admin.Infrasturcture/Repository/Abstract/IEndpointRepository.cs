using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.Endpoint;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IEndpointRepository
    {
        Task DeleteEndpoint(int id);
        Task<Endpoint> GetEndpointById(int id);
        Task UpdateEndpoint(int id, UpdateEndpointDto model);
        Task<IEnumerable<EndpointResponseModel>> GetEndpointsByIdsAsync(IEnumerable<int> ids);
        Task<PaginatedResult<EndpointResponseModel>> GetAllEndpoints(EndpointFilter roleFilter);
        Task<Endpoint> CreateEndpointAsync(string path, string description = null, string? endpointType = null);
    }
}

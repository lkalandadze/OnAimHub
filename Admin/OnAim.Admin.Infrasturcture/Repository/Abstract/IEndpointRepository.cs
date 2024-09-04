using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.Endpoint;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IEndpointRepository
    {
        Task<PaginatedResult<EndpointResponseModel>> GetAllEndpoints(EndpointFilter roleFilter);
    }
}

using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.Infrasturcture.Repository.Abstract
{
    public interface IEndpointRepository
    {
        Task<PaginatedResult<EndpointResponseModel>> GetAllEndpoints(EndpointFilter roleFilter);
    }
}

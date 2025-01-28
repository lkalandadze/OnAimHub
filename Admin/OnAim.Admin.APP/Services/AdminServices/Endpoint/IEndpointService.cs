using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Endpoint;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Services.AdminServices.Endpoint;

public interface IEndpointService
{
    Task<ApplicationResult<string>> Create(List<CreateEndpointDto> endpoints);
    Task<ApplicationResult<bool>> Delete(List<int> ids);
    Task<ApplicationResult<string>> Update(int id, UpdateEndpointDto endpoint);
    Task<ApplicationResult<PaginatedResult<EndpointResponseModel>>> GetAll(EndpointFilter filter);
    Task<ApplicationResult<EndpointResponseModel>> GetById(int id);
}
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IEndpointService
{
    Task<ApplicationResult> Create(List<CreateEndpointDto> endpoints);
    Task<ApplicationResult> Delete(List<int> ids);
    Task<ApplicationResult> Update(int id, UpdateEndpointDto endpoint);
    Task<ApplicationResult> GetAll(EndpointFilter filter);
    Task<ApplicationResult> GetById(int id);
}
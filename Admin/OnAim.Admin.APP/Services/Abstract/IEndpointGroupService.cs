using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.EndpointGroup;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IEndpointGroupService
{
    Task<ApplicationResult> Create(CreateEndpointGroupRequest model);
    Task<ApplicationResult> Delete(List<int> ids);
    Task<ApplicationResult> Update(int id, UpdateEndpointGroupRequest model);
    Task<ApplicationResult> GetAll(EndpointGroupFilter filter);
    Task<ApplicationResult> GetById(int id);
}
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.APP.Commands.EndpointGroup.GroupBondToEndpoint
{
    public record GroupBondToEndpointCommand(int GroupId, List<EndpointDto>? Endpoints, List<RoleDto>? Roles) : ICommand<ApplicationResult>;
    public record EndpointDto(int Id, bool IsActive);
}

using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Update;

public record UpdateRoleCommand(int Id, UpdateRoleRequest Model) : ICommand<ApplicationResult>;

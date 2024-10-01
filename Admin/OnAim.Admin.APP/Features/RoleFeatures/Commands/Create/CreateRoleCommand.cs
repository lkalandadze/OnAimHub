using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Create;

public record CreateRoleCommand(CreateRoleRequest Request) : ICommand<ApplicationResult>;

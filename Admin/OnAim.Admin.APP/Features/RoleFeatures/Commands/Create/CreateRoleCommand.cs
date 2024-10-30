using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Role;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Create;

public record CreateRoleCommand(CreateRoleRequest Request) : ICommand<ApplicationResult>;

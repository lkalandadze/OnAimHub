using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Role;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Update;

public record UpdateRoleCommand(int Id, UpdateRoleRequest Model) : ICommand<ApplicationResult>;

using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Delete;

public record DeleteRoleCommand(List<int> Ids) : ICommand<ApplicationResult>;

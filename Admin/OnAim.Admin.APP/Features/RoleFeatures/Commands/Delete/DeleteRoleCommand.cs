using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Delete;

public record DeleteRoleCommand(int Id) : ICommand<ApplicationResult>;

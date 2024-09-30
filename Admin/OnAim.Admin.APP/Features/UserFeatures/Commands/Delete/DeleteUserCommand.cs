using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Delete;

public record DeleteUserCommand(List<int> UserIds) : ICommand<ApplicationResult>;

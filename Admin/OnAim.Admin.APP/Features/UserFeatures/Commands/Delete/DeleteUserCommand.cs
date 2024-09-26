using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Delete;

public record DeleteUserCommand(int UserId) : ICommand<ApplicationResult>;

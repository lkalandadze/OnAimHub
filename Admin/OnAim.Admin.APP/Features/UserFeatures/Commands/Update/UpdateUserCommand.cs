using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Update;

public sealed record UpdateUserCommand(int Id, UpdateUserRequest Model) : ICommand<ApplicationResult>;

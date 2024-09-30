using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Update;

public sealed record UpdateUserCommand(int Id, UpdateUserRequest Model) : ICommand<ApplicationResult>;

using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Enums;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Activate;

public record ActivateAccountCommand(string Email, string Code) : ICommand<ApplicationResult>;
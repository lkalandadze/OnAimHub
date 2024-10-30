using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ChangePassword;

public record ChangePasswordCommand(string Email, string OldPassword, string NewPassword) : ICommand<ApplicationResult>;

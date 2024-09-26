using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ChangePassword;

public record ChangePasswordCommand(string Email, string OldPassword, string NewPassword) : ICommand<ApplicationResult>;

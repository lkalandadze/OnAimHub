using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.Dtos.User;

namespace OnAim.Admin.APP.Features.UserFeatures.Commands.TwoFA;

public record VerifyOtpCommand(string Email, string OtpCode) : ICommand<AuthResultDto>;
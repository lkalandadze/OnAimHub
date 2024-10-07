using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.DTOs.User;

namespace OnAim.Admin.APP.Features.UserFeatures.Commands.TwoFA;

public record VerifyOtpCommand(string Email, string OtpCode) : ICommand<AuthResultDto>;
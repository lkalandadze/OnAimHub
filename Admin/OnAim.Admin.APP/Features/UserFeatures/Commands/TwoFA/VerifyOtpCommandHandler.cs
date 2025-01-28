using ValidationException = FluentValidation.ValidationException;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.AdminServices.User;
using OnAim.Admin.Contracts.Dtos.User;

namespace OnAim.Admin.APP.Features.UserFeatures.Commands.TwoFA;

public class VerifyOtpCommandHandler : ICommandHandler<VerifyOtpCommand, AuthResultDto>
{
    private readonly IUserService _userService;
    private readonly IValidator<VerifyOtpCommand> _validator;

    public VerifyOtpCommandHandler(IUserService userService, IValidator<VerifyOtpCommand> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<AuthResultDto> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _userService.TwoFA(request.Email, request.OtpCode);

        return new AuthResultDto { 
            AccessToken = result.AccessToken, 
            Expiry = result.Expiry,
            StatusCode = result.StatusCode,
            Message = result.Message,
            RefreshToken = result.RefreshToken,
        };
    }
}

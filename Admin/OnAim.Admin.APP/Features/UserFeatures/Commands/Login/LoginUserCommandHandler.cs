using OnAim.Admin.Contracts.Dtos.User;
using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.APP.Services.AdminServices.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Login;

public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, AuthResultDto>
{
    private readonly IUserService _userService;
    private readonly IValidator<LoginUserCommand> _validator;

    public LoginUserCommandHandler(IUserService userService, IValidator<LoginUserCommand> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<AuthResultDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _userService.Login(request.Model);

        return new AuthResultDto 
        { 
            AccessToken = result.AccessToken, 
            Expiry = result.Expiry, 
            Message = result.Message, 
            RefreshToken = result.RefreshToken, 
            StatusCode = result.StatusCode 
        };
    }
}
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using FluentValidation;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, ApplicationResult>
{
    private readonly IUserService _userService;
    private readonly IValidator<ForgotPasswordCommand> _validator;

    public ForgotPasswordCommandHandler(IUserService userService, IValidator<ForgotPasswordCommand> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _userService.ForgotPassword(request.Email);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}

using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.AdminServices.User;
using ValidationException = FluentValidation.ValidationException;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ForgotPassword;

public class ResetPasswordHandler : ICommandHandler<ResetPassword, ApplicationResult<bool>>
{
    private readonly IUserService _userService;
    private readonly IValidator<ResetPassword> _validator;

    public ResetPasswordHandler(IUserService userService, IValidator<ResetPassword> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(ResetPassword request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _userService.ResetPassword(request.Email, request.Code, request.Password);
    }
}

using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.Services.AdminServices.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ChangePassword;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, ApplicationResult>
{
    private readonly IUserService _userService;
    private readonly IValidator<ChangePasswordCommand> _validator;

    public ChangePasswordCommandHandler(IUserService userService, IValidator<ChangePasswordCommand> validator )
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _userService.ChangePassword(request.Email, request.OldPassword, request.NewPassword);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}

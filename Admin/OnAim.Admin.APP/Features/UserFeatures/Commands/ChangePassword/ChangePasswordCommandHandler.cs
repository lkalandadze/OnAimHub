﻿using OnAim.Admin.APP.CQRS.Command;
using ValidationException = FluentValidation.ValidationException;
using OnAim.Admin.APP.Services.AdminServices.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ChangePassword;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, ApplicationResult<bool>>
{
    private readonly IUserService _userService;
    private readonly IValidator<ChangePasswordCommand> _validator;

    public ChangePasswordCommandHandler(IUserService userService, IValidator<ChangePasswordCommand> validator )
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _userService.ChangePassword(request.Email, request.OldPassword, request.NewPassword);
    }
}

﻿using OnAim.Admin.APP.CQRS.Command;
using ValidationException = FluentValidation.ValidationException;
using OnAim.Admin.APP.Services.AdminServices.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Activate;

public class ActivateAccountCommandHandler : ICommandHandler<ActivateAccountCommand, ApplicationResult<string>>
{
    private readonly IUserService _userService;
    private readonly IValidator<ActivateAccountCommand> _validator;

    public ActivateAccountCommandHandler(IUserService userService, IValidator<ActivateAccountCommand> validator) 
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult<string>> Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _userService.ActivateAccount(request.Email, request.Code);
    }
}

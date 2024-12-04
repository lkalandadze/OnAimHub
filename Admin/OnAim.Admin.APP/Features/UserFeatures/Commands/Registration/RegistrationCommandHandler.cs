using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.APP.Services.AdminServices.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Registration;

public class RegistrationCommandHandler : ICommandHandler<RegistrationCommand, ApplicationResult>
{
    private readonly IUserService _userService;
    private readonly IValidator<RegistrationCommand> _validator;

    public RegistrationCommandHandler(IUserService userService, IValidator<RegistrationCommand> validator) 
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(RegistrationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _userService.Registration(request.Email, request.Password, request.FirstName, request.LastName, request.Phone, request.DateOfBirth);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}

using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using ValidationException = FluentValidation.ValidationException;
using OnAim.Admin.APP.Services.AdminServices.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Registration;

public class RegistrationCommandHandler : ICommandHandler<RegistrationCommand, ApplicationResult<bool>>
{
    private readonly IUserService _userService;
    private readonly IValidator<RegistrationCommand> _validator;

    public RegistrationCommandHandler(IUserService userService, IValidator<RegistrationCommand> validator) 
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _userService.Registration(request.Email, request.Password, request.FirstName, request.LastName, request.Phone, request.DateOfBirth);
    }
}

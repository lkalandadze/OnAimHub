using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.APP.Services.AdminServices.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Create;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, ApplicationResult<bool>>
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserCommand> _validator;

    public CreateUserCommandHandler(IUserService userService, IValidator<CreateUserCommand> validator) 
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _userService.Create(request.Email, request.FirstName, request.LastName, request.Phone);
    }
}

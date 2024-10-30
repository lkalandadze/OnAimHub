using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using FluentValidation;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Create;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, ApplicationResult>
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserCommand> _validator;

    public CreateUserCommandHandler(IUserService userService, IValidator<CreateUserCommand> validator) 
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _userService.Create(request.Email, request.FirstName, request.LastName, request.Phone);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}

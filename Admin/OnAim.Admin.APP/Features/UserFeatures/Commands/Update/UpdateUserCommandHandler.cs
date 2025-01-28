using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.APP.Services.AdminServices.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Update;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, ApplicationResult<string>>
{
    private readonly IUserService _userService;
    private readonly IValidator<UpdateUserCommand> _validator;

    public UpdateUserCommandHandler(IUserService userService, IValidator<UpdateUserCommand> validator) 
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _userService.Update(request.Id, request.Model);
    }
}

using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using FluentValidation;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Update;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, ApplicationResult>
{
    private readonly IUserService _userService;
    private readonly IValidator<UpdateUserCommand> _validator;

    public UpdateUserCommandHandler(IUserService userService, IValidator<UpdateUserCommand> validator) 
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _userService.Update(request.Id, request.Model);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}

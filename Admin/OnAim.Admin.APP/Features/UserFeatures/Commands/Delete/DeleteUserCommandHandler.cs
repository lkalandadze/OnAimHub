using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using ValidationException = FluentValidation.ValidationException;
using OnAim.Admin.APP.Services.AdminServices.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Delete;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, ApplicationResult<bool>>
{
    private readonly IUserService _userService;
    private readonly IValidator<DeleteUserCommand> _validator;

    public DeleteUserCommandHandler(IUserService userService, IValidator<DeleteUserCommand> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _userService.Delete(request.UserIds);
    }
}

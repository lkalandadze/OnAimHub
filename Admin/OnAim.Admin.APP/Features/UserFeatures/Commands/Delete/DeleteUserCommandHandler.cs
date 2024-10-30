using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using FluentValidation;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Delete;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, ApplicationResult>
{
    private readonly IUserService _userService;
    private readonly IValidator<DeleteUserCommand> _validator;

    public DeleteUserCommandHandler(IUserService userService, IValidator<DeleteUserCommand> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _userService.Delete(request.UserIds);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}

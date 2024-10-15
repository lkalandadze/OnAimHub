using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using FluentValidation;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ProfileUpdate;

public class UserProfileUpdateCommandHandler : ICommandHandler<UserProfileUpdateCommand, ApplicationResult>
{
    private readonly IUserService _userService;
    private readonly IValidator<UserProfileUpdateCommand> _validator;

    public UserProfileUpdateCommandHandler(IUserService userService, IValidator<UserProfileUpdateCommand> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(UserProfileUpdateCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _userService.ProfileUpdate(request.Id, request.ProfileUpdateRequest);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}

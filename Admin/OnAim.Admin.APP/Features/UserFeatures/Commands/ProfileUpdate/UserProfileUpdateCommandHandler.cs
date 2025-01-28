using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.APP.Services.AdminServices.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ProfileUpdate;

public class UserProfileUpdateCommandHandler : ICommandHandler<UserProfileUpdateCommand, ApplicationResult<bool>>
{
    private readonly IUserService _userService;
    private readonly IValidator<UserProfileUpdateCommand> _validator;

    public UserProfileUpdateCommandHandler(IUserService userService, IValidator<UserProfileUpdateCommand> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(UserProfileUpdateCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _userService.ProfileUpdate(request.Id, request.ProfileUpdateRequest);
    }
}

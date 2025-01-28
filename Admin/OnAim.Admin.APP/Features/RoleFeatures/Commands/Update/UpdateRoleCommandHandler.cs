using OnAim.Admin.APP.CQRS.Command;
using ValidationException = FluentValidation.ValidationException;
using OnAim.Admin.APP.Services.AdminServices.Role;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Update;

public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand, ApplicationResult<string>>
{
    private readonly IRoleService _roleService;
    private readonly IValidator<UpdateRoleCommand> _validator;

    public UpdateRoleCommandHandler(IRoleService roleService, IValidator<UpdateRoleCommand> validator) 
    {
        _roleService = roleService;
        _validator = validator;
    }

    public async Task<ApplicationResult<string>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _roleService.Update(request.Id, request.Model);
    }
}

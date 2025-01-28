using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.APP.Services.AdminServices.Role;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Create;

public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, ApplicationResult<string>>
{
    private readonly IRoleService _roleService;
    private readonly IValidator<CreateRoleCommand> _validator;

    public CreateRoleCommandHandler(IRoleService roleService, IValidator<CreateRoleCommand> validator) 
    {
        _roleService = roleService;
        _validator = validator;
    }

    public async Task<ApplicationResult<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _roleService.Create(request.Request);
    }
}

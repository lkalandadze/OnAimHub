using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using FluentValidation;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Create;

public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, ApplicationResult>
{
    private readonly IRoleService _roleService;
    private readonly IValidator<CreateRoleCommand> _validator;

    public CreateRoleCommandHandler(IRoleService roleService, IValidator<CreateRoleCommand> validator) 
    {
        _roleService = roleService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _roleService.Create(request.Request);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}

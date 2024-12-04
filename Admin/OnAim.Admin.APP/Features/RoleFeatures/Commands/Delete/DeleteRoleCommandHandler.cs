using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.APP.Services.AdminServices.Role;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Delete;

public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, ApplicationResult>
{
    private readonly IRoleService _roleService;
    private readonly IValidator<DeleteRoleCommand> _validator;

    public DeleteRoleCommandHandler(IRoleService roleService, IValidator<DeleteRoleCommand> validator)
    {
        _roleService = roleService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if(!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _roleService.Delete(request.Ids);

        return new ApplicationResult { Success = result.Success };
    }
}

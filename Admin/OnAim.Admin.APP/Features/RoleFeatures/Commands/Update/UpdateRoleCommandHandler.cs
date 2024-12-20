﻿using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.APP.Services.AdminServices.Role;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Update;

public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand, ApplicationResult>
{
    private readonly IRoleService _roleService;
    private readonly IValidator<UpdateRoleCommand> _validator;

    public UpdateRoleCommandHandler(IRoleService roleService, IValidator<UpdateRoleCommand> validator) 
    {
        _roleService = roleService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _roleService.Update(request.Id, request.Model);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}

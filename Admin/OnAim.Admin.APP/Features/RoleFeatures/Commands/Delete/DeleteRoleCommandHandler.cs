using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.Domain.Exceptions;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Delete;

public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, ApplicationResult>
{
    private readonly IRepository<Role> _repository;
    private readonly IValidator<DeleteRoleCommand> _validator;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public DeleteRoleCommandHandler(
        IRepository<Role> repository,
        IValidator<DeleteRoleCommand> validator,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _repository = repository;
        _validator = validator;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var role = await _repository.Query(x => x.Id == request.Id)
            .Include(r => r.UserRoles)
            .Include(r => r.RoleEndpointGroups)
            .FirstOrDefaultAsync();

        if (role == null)
        {
            throw new NotFoundException("Role Not Found");
        }

        role.IsActive = false;
        role.IsDeleted = true;
        role.DateDeleted = SystemDate.Now;

        await _repository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
        };
    }
}

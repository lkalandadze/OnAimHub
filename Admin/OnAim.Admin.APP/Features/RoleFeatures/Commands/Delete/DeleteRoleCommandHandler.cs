using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Domain.Exceptions;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Delete;

public class DeleteRoleCommandHandler : BaseCommandHandler<DeleteRoleCommand, ApplicationResult>
{
    private readonly IRepository<Role> _repository;

    public DeleteRoleCommandHandler(CommandContext<DeleteRoleCommand> context,IRepository<Role> repository) : base( context )
    {
        _repository = repository;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var roles = await _repository.Query(x => request.Ids.Contains(x.Id)).ToListAsync();

        if (!roles.Any())
            throw new NotFoundException("Role Not Found");

        foreach ( var role in roles)
        {
            role.IsActive = false;
            role.MarkAsDeleted();
        }

        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }
}

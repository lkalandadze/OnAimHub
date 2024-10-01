using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Delete;

public class DeleteUserCommandHandler : BaseCommandHandler<DeleteUserCommand, ApplicationResult>
{
    private readonly IRepository<User> _repository;

    public DeleteUserCommandHandler(
        CommandContext<DeleteUserCommand> context,
        IRepository<User> repository
        ) : base( context )
    {
        _repository = repository;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var users = await _repository.Query(x => request.UserIds.Contains(x.Id)).ToListAsync(cancellationToken);

        if (!users.Any())
            throw new NotFoundException("No users found for the provided IDs");

        foreach (var user in users)
        {
            user.IsActive = false;
            user.MarkAsDeleted();
        }

        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }
}

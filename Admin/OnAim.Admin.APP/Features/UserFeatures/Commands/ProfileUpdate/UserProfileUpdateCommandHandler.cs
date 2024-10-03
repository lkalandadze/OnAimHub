using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ProfileUpdate;

public class UserProfileUpdateCommandHandler : BaseCommandHandler<UserProfileUpdateCommand, ApplicationResult>
{
    private readonly IRepository<User> _repository;

    public UserProfileUpdateCommandHandler(
        CommandContext<UserProfileUpdateCommand> context,
        IRepository<User> repository
        ) : base( context )
    {
        _repository = repository;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(UserProfileUpdateCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var user = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            throw new NotFoundException("User Not Found");

        user.FirstName = request.profileUpdateRequest.FirstName;
        user.LastName = request.profileUpdateRequest.LastName;
        user.Phone = request.profileUpdateRequest.Phone;
        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }
}

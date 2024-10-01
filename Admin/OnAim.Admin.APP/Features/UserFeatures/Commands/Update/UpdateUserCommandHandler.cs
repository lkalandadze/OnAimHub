using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Update;

public class UpdateUserCommandHandler : BaseCommandHandler<UpdateUserCommand, ApplicationResult>
{
    private readonly IRepository<User> _repository;
    private readonly IConfigurationRepository<UserRole> _userRoleRepository;

    public UpdateUserCommandHandler(
        CommandContext<UpdateUserCommand> context,
        IRepository<User> repository,
        IConfigurationRepository<UserRole> userRoleRepository
        ) : base( context )
    {
        _repository = repository;
        _userRoleRepository = userRoleRepository;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var existingUser = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

        if (existingUser == null)
            throw new NotFoundException("User not found");           

        existingUser.UpdateUserDetails(request.Model.FirstName, request.Model.LastName, request.Model.Phone, request.Model.IsActive);

        var currentRoles = await _userRoleRepository.Query(ur => ur.UserId == request.Id).ToListAsync();

        var currentRoleIds = currentRoles.Select(ur => ur.RoleId).ToHashSet();
        var newRoleIds = request.Model.RoleIds?.ToHashSet() ?? new HashSet<int>();

        var rolesToAdd = newRoleIds.Except(currentRoleIds).ToList();
        foreach (var roleId in rolesToAdd)
        {
            var userRole = new UserRole(request.Id, roleId);
            await _userRoleRepository.Store(userRole);
        }

        var rolesToRemove = currentRoleIds.Except(newRoleIds).ToList();
        foreach (var roleId in rolesToRemove)
        {
            var userRole = await _userRoleRepository
                .Query(ur => ur.UserId == request.Id && ur.RoleId == roleId).FirstOrDefaultAsync();
            if (userRole != null)
            {
                await _userRoleRepository.Remove(userRole);
            }
        }

        await _repository.CommitChanges();
        await _userRoleRepository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = $"User {existingUser.FirstName} {existingUser.LastName} Updated Successfully"
        };
    }
}

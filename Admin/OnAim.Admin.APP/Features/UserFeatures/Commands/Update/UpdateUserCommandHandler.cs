using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.Shared.Helpers;
using OnAim.Admin.APP.CQRS;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Update;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, ApplicationResult>
{
    private readonly IRepository<User> _repository;
    private readonly IConfigurationRepository<UserRole> _userRoleRepository;
    private readonly IValidator<UpdateUserCommand> _validator;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public UpdateUserCommandHandler(
        IRepository<User> repository,
        IConfigurationRepository<UserRole> userRoleRepository,
        IValidator<UpdateUserCommand> validator,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _repository = repository;
        _userRoleRepository = userRoleRepository;
        _validator = validator;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);      

        var existingUser = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

        if (existingUser == null)
            throw new NotFoundException("User not found");           

        var userClone = new User
        {
            Id = existingUser.Id,
            FirstName = existingUser.FirstName,
            LastName = existingUser.LastName,
            Phone = existingUser.Phone,
            IsActive = existingUser.IsActive,
            UserRoles = existingUser.UserRoles,
        };

        existingUser.FirstName = request.Model.FirstName;
        existingUser.LastName = request.Model.LastName;
        existingUser.Phone = request.Model.Phone;
        existingUser.DateUpdated = SystemDate.Now;
        existingUser.IsActive = request.Model.IsActive ?? true;

        var currentRoles = await _userRoleRepository.Query(ur => ur.UserId == request.Id).ToListAsync();

        var currentRoleIds = currentRoles.Select(ur => ur.RoleId).ToHashSet();
        var newRoleIds = request.Model.RoleIds?.ToHashSet() ?? new HashSet<int>();

        var rolesToAdd = newRoleIds.Except(currentRoleIds).ToList();
        foreach (var roleId in rolesToAdd)
        {
            var userRole = new UserRole { UserId = request.Id, RoleId = roleId };
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

        var changeLog = AuditLogger.GenerateChangeLog(userClone, existingUser);

        return new ApplicationResult
        {
            Success = true,
            Data = $"User {existingUser.FirstName} {existingUser.LastName} Updated Successfully"
        };
    }
}

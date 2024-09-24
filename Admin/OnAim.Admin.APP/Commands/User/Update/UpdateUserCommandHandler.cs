using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.Exceptions;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.Shared.Helpers;

namespace OnAim.Admin.APP.Commands.User.Update
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _repository;
        private readonly IConfigurationRepository<UserRole> _userRoleRepository;
        private readonly IValidator<UpdateUserCommand> _validator;
        private readonly IAuditLogService _auditLogService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public UpdateUserCommandHandler(
            IRepository<Infrasturcture.Entities.User> repository,
            IConfigurationRepository<UserRole> userRoleRepository,
            IValidator<UpdateUserCommand> validator,
            IAuditLogService auditLogService,
            ISecurityContextAccessor securityContextAccessor
            )
        {
            _repository = repository;
            _userRoleRepository = userRoleRepository;
            _validator = validator;
            _auditLogService = auditLogService;
            _securityContextAccessor = securityContextAccessor;
        }
        public async Task<ApplicationResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var existingUser = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found");
            }

            var userClone = new Infrasturcture.Entities.User
            {
                Id = existingUser.Id,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                Phone = existingUser.Phone
            };

            existingUser.FirstName = request.Model.FirstName;
            existingUser.LastName = request.Model.LastName;
            existingUser.Phone = request.Model.Phone;
            existingUser.DateUpdated = SystemDate.Now;
            existingUser.IsActive = request.Model.IsActive ?? true;

            var currentRoles = await _userRoleRepository
            .Query(ur => ur.UserId == request.Id)
                   .ToListAsync();

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

            var auditLog = new AuditLog
            {
                UserId = _securityContextAccessor.UserId,
                Timestamp = SystemDate.Now,
                Action = "UPDATE",
                ObjectId = existingUser.Id,
                Category = "User",
                Log = $"User updated successfully with ID: {existingUser.Id} by User ID: {_securityContextAccessor.UserId}. Changes: {changeLog}"
            };

            await _auditLogService.LogEventAsync(auditLog);

            return new ApplicationResult
            {
                Success = true,
                Data = $"User {existingUser.FirstName} {existingUser.LastName} Updated Successfully"
            };
        }
    }
}

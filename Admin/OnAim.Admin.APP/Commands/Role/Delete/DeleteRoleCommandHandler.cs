using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using static OnAim.Admin.Shared.Exceptions.Exceptions;

namespace OnAim.Admin.APP.Commands.Role.Delete
{
    public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.Role> _repository;
        private readonly IValidator<DeleteRoleCommand> _validator;
        private readonly IAuditLogService _auditLogService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public DeleteRoleCommandHandler(
            IRepository<Infrasturcture.Entities.Role> repository,
            IValidator<DeleteRoleCommand> validator,
            IAuditLogService auditLogService,
            ISecurityContextAccessor securityContextAccessor
            )
        {
            _repository = repository;
            _validator = validator;
            _auditLogService = auditLogService;
            _securityContextAccessor = securityContextAccessor;
        }
        public async Task<ApplicationResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var role = await _repository.Query(x => x.Id == request.Id)
                .Include(r => r.UserRoles)
                .Include(r => r.RoleEndpointGroups)
                .FirstOrDefaultAsync();

            if (role == null)
            {
                throw new RoleNotFoundException("Role Not Found");
            }

            role.IsActive = false;
            role.IsDeleted = true;
            role.DateDeleted = SystemDate.Now;

            await _repository.CommitChanges();

            var auditLog = new AuditLog
            {
                UserId = _securityContextAccessor.UserId,
                Timestamp = SystemDate.Now,
                Action = "DELETE_ROLE",
                ObjectId = role.Id,
                Log = $"Role Deleted successfully with ID: {role.Id} by User ID: {_securityContextAccessor.UserId}"
            };

            await _auditLogService.LogEventAsync(auditLog);

            return new ApplicationResult
            {
                Success = true,
            };
        }
    }
}

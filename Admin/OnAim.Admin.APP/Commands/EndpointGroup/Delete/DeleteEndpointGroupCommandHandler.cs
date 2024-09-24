using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Exceptions;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Delete
{
    public class DeleteEndpointGroupCommandHandler : ICommandHandler<DeleteEndpointGroupCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.EndpointGroup> _repository;
        private readonly IValidator<DeleteEndpointGroupCommand> _validator;
        private readonly IAuditLogService _auditLogService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public DeleteEndpointGroupCommandHandler(
            IRepository<Infrasturcture.Entities.EndpointGroup> repository,
            IValidator<DeleteEndpointGroupCommand> validator,
            IAuditLogService auditLogService,
            ISecurityContextAccessor securityContextAccessor
            )
        {
            _repository = repository;
            _validator = validator;
            _auditLogService = auditLogService;
            _securityContextAccessor = securityContextAccessor;
        }
        public async Task<ApplicationResult> Handle(DeleteEndpointGroupCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var group = await _repository.Query(x => x.Id == request.GroupId).FirstOrDefaultAsync();

            if (group == null)
            {
                throw new NotFoundException("Permission Group Not Found");
            }

            group.IsDeleted = true;
            group.IsActive = false;
            group.DateDeleted = SystemDate.Now;

            var auditLog = new AuditLog
            {
                UserId = _securityContextAccessor.UserId,
                Timestamp = SystemDate.Now,
                Action = "DELETE",
                ObjectId = group.Id,
                Category = "EndpointGroup",
                Log = $"EndpointGroup Deleted successfully with ID: {group.Id} by User ID: {_securityContextAccessor.UserId}"
            };

            await _auditLogService.LogEventAsync(auditLog);

            await _repository.CommitChanges();

            return new ApplicationResult { Success = true };
        }
    }
}

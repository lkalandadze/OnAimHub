using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Exceptions;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.EndPoint.Delete
{
    public class DeleteEndpointCommandHandler : ICommandHandler<DeleteEndpointCommand, ApplicationResult>
    {
        private readonly IRepository<Endpoint> _repository;
        private readonly IValidator<DeleteEndpointCommand> _validator;
        private readonly IAuditLogService _auditLogService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public DeleteEndpointCommandHandler(
            IRepository<Endpoint> repository,
            IValidator<DeleteEndpointCommand> validator,
            IAuditLogService auditLogService,
            ISecurityContextAccessor securityContextAccessor
            )
        {
            _repository = repository;
            _validator = validator;
            _auditLogService = auditLogService;
            _securityContextAccessor = securityContextAccessor;
        }
        public async Task<ApplicationResult> Handle(DeleteEndpointCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var endpoint = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (endpoint == null)
            {
                throw new EndpointNotFoundException("Permission Not Found");
            }

            endpoint.IsActive = false;
            endpoint.IsDeleted = true;
            endpoint.DateDeleted = SystemDate.Now;
            await _repository.CommitChanges();

            await _auditLogService.LogEventAsync(
                  SystemDate.Now,
                  "Endpoint Deletion",
                  nameof(Endpoint),
                  endpoint.Id,
                  _securityContextAccessor.UserId,
                  $"Endpoint Deleted successfully with ID: {endpoint.Id} by User ID: {_securityContextAccessor.UserId}");

            return new ApplicationResult { Success = true };
        }
    }
}

﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Exceptions;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.EndPoint.Update
{
    public sealed class UpdateEndpointCommandHandler : ICommandHandler<UpdateEndpointCommand, ApplicationResult>
    {
        private readonly IRepository<Endpoint> _repository;
        private readonly IValidator<UpdateEndpointCommand> _validator;
        private readonly IAuditLogService _auditLogService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public UpdateEndpointCommandHandler(
            IRepository<Endpoint> repository,
            IValidator<UpdateEndpointCommand> validator,
            IAuditLogService auditLogService,
            ISecurityContextAccessor securityContextAccessor
            )
        {
            _repository = repository;
            _validator = validator;
            _auditLogService = auditLogService;
            _securityContextAccessor = securityContextAccessor;
        }
        public async Task<ApplicationResult> Handle(UpdateEndpointCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var ep = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (ep == null)
            {
                throw new EndpointNotFoundException("Permission Not Found");
            }

            ep.Description = request.Endpoint.Description;
            ep.IsActive = request.Endpoint.IsActive ?? true;
            ep.IsDeleted = request.Endpoint.IsEnabled ?? true;
            ep.DateUpdated = SystemDate.Now;

            await _repository.CommitChanges();

            await _auditLogService.LogEventAsync(
                  SystemDate.Now,
                  "Endpoint Update",
                  nameof(Endpoint),
                  ep.Id,
                  _securityContextAccessor.UserId,
                  $"Endpoint Updated successfully with ID: {ep.Id} by User ID: {_securityContextAccessor.UserId}");

            return new ApplicationResult
            {
                Success = true,
                Data = $"Permmission {ep.Name} Successfully Updated"
            };
        }
    }
}

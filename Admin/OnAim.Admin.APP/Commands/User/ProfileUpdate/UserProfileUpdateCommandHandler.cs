﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.Exceptions;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.User.ProfileUpdate
{
    public class UserProfileUpdateCommandHandler : ICommandHandler<UserProfileUpdateCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _repository;
        private readonly IValidator<UserProfileUpdateCommand> _validator;
        private readonly IAuditLogService _auditLogService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public UserProfileUpdateCommandHandler(
            IRepository<Infrasturcture.Entities.User> repository,
            IValidator<UserProfileUpdateCommand> validator,
            IAuditLogService auditLogService,
            ISecurityContextAccessor securityContextAccessor
            )
        {
            _repository = repository;
            _validator = validator;
            _auditLogService = auditLogService;
            _securityContextAccessor = securityContextAccessor;
        }
        public async Task<ApplicationResult> Handle(UserProfileUpdateCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var user = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new UserNotFoundException("User Not Found");
            }

            user.FirstName = request.profileUpdateRequest.FirstName;
            user.LastName = request.profileUpdateRequest.LastName;
            user.Phone = request.profileUpdateRequest.Phone;
            user.DateUpdated = SystemDate.Now;

            await _repository.CommitChanges();

            var auditLog = new AuditLog
            {
                UserId = _securityContextAccessor.UserId,
                Timestamp = SystemDate.Now,
                Action = "UPDATE_PROFILE",
                ObjectId = user.Id,
                Log = $"User Profile Updated successfully with ID: {user.Id}"
            };

            await _auditLogService.LogEventAsync(auditLog);

            return new ApplicationResult { Success = true };
        }
    }
}

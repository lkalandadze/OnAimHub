using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Exceptions;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.User.Activate
{
    public class ActivateAccountCommandHandler : ICommandHandler<ActivateAccountCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _repository;
        private readonly IAuditLogService _auditLogService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public ActivateAccountCommandHandler(
            IRepository<Infrasturcture.Entities.User> repository,
            IAuditLogService auditLogService,
            ISecurityContextAccessor securityContextAccessor
            )
        {
            _repository = repository;
            _auditLogService = auditLogService;
            _securityContextAccessor = securityContextAccessor;
        }
        public async Task<ApplicationResult> Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.Query(x => x.Email == request.Email && !x.IsDeleted).FirstOrDefaultAsync();

            if (user == null || user.ActivationCode != request.Code)
            {
                throw new BadRequestException("Invalid activation code.");
            }

            if (user.ActivationCodeExpiration < DateTime.UtcNow)
            {
                throw new BadRequestException("Activation code has expired.");
            }

            user.IsActive = true;
            user.IsVerified = true;
            user.ActivationCode = null;
            user.ActivationCodeExpiration = null;

            await _repository.CommitChanges();

            var auditLog = new AuditLog
            {
                UserId = _securityContextAccessor.UserId,
                Timestamp = SystemDate.Now,
                Action = "ACTIVATION",
                ObjectId = user.Id,
                Category = "User",
                Log = $"User was Activated successfully with ID: {user.Id}"
            };

            await _auditLogService.LogEventAsync(auditLog);

            return new ApplicationResult { Success = true, Data = "Account activated successfully." };
        }
    }
}

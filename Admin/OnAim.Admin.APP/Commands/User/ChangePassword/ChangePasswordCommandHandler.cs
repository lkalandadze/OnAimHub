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

namespace OnAim.Admin.APP.Commands.User.ChangePassword
{
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _userRepository;
        private readonly IValidator<ChangePasswordCommand> _validator;
        private readonly IAuditLogService _auditLogService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public ChangePasswordCommandHandler(
            IRepository<Infrasturcture.Entities.User> userRepository,
            IValidator<ChangePasswordCommand> validator,
            IAuditLogService auditLogService,
            ISecurityContextAccessor securityContextAccessor
            )
        {
            _userRepository = userRepository;
            _validator = validator;
            _auditLogService = auditLogService;
            _securityContextAccessor = securityContextAccessor;
        }
        public async Task<ApplicationResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.Query(x => x.Email == request.Email).FirstOrDefaultAsync();

            if (user == null || !user.IsActive)
            {
                throw new UserNotFoundException("User Not Found!");
            }

            string hashedOldPassword = EncryptPasswordExtension.EncryptPassword(request.OldPassword, user.Salt);

            if (user.Password != hashedOldPassword)
            {
                throw new BadRequestException("Old password is incorrect!");
            }

            var newSalt = EncryptPasswordExtension.Salt();
            string hashedNewPassword = EncryptPasswordExtension.EncryptPassword(request.NewPassword, newSalt);

            user.Password = hashedNewPassword;
            user.Salt = newSalt;
            user.DateUpdated = SystemDate.Now;

            try
            {
                await _userRepository.CommitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the password.");
            }

            var auditLog = new AuditLog
            {
                UserId = _securityContextAccessor.UserId,
                Timestamp = SystemDate.Now,
                Action = "PASSWORD_CHANGED",
                ObjectId = user.Id,
                Log = $"Password was Changed successfully with ID: {user.Id} by User ID: {_securityContextAccessor.UserId}"
            };

            await _auditLogService.LogEventAsync(auditLog);

            return new ApplicationResult
            {
                Success = true,
            };
        }
    }
}

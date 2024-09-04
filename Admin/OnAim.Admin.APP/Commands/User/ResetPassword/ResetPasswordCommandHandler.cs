using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Extensions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.User.ResetPassword
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _userRepository;
        private readonly IValidator<ResetPasswordCommand> _validator;
        private readonly IEmailService _emailService;
        private readonly IAuditLogService _auditLogService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public ResetPasswordCommandHandler(
            IRepository<Infrasturcture.Entities.User> userRepository,
            IValidator<ResetPasswordCommand> validator,
            IEmailService emailService,
            IAuditLogService auditLogService,
            ISecurityContextAccessor securityContextAccessor
            )
        {
            _userRepository = userRepository;
            _validator = validator;
            _emailService = emailService;
            _auditLogService = auditLogService;
            _securityContextAccessor = securityContextAccessor;
        }

        public async Task<ApplicationResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (user != null && user.IsActive)
            {
                var salt = EncryptPasswordExtension.Salt();

                string hashed = EncryptPasswordExtension.EncryptPassword(request.Password, salt);

                user.Password = hashed;
                user.Salt = salt;

                await _userRepository.CommitChanges();

                await _emailService.SendActivationEmailAsync(
                      user.Email,
                      "Your Account is Created",
                request.Password,
                      user.FirstName
                );
            }

            await _auditLogService.LogEventAsync(
               SystemDate.Now,
               "Password Reset",
               nameof(User),
               user.Id,
               _securityContextAccessor.UserId,
               $"User Password was Reseted successfully with ID: {user.Id} by User ID: {_securityContextAccessor.UserId}");

            return new ApplicationResult
            {
                Success = true,
            };
        }
    }
}

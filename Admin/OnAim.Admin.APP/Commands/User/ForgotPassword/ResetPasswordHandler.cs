using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Helpers;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.User.ForgotPassword
{
    public class ResetPasswordHandler : ICommandHandler<ResetPassword, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _userRepository;
        private readonly IAuditLogService _auditLogService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public ResetPasswordHandler(
            IRepository<Infrasturcture.Entities.User> userRepository,
             IAuditLogService auditLogService,
            ISecurityContextAccessor securityContextAccessor
            )
        {
            _userRepository = userRepository;
            _auditLogService = auditLogService;
            _securityContextAccessor = securityContextAccessor;
        }

        public async Task<ApplicationResult> Handle(ResetPassword request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Query(x =>
                 x.Email == request.Email &&
                 x.ResetCode == request.Code &&
                 x.ResetCodeExpiration > DateTime.UtcNow &&
                 !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new Exception("Invalid or expired reset token.");
            }

            var salt = EncryptPasswordExtension.Salt();
            var hashedPassword = EncryptPasswordExtension.EncryptPassword(request.Password, salt);

            user.Password = hashedPassword;
            user.Salt = salt;
            user.ResetCode = null;
            user.ResetCodeExpiration = null;

            await _userRepository.CommitChanges();

            var auditLog = new AuditLog
            {
                UserId = _securityContextAccessor.UserId,
                Timestamp = SystemDate.Now,
                Action = "FORGOT_PASSWORD",
                ObjectId = user.Id,
                Category = "User",
                Log = $"User Password Changed successfully with ID: {user.Id}"
            };

            await _auditLogService.LogEventAsync(auditLog);

            return new ApplicationResult { Success = true };
        }
    }
}

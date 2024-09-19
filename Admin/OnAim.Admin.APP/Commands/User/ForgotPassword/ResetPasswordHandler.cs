using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Helpers;

namespace OnAim.Admin.APP.Commands.User.ForgotPassword
{
    public class ResetPasswordHandler : ICommandHandler<ResetPassword, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _userRepository;

        public ResetPasswordHandler(IRepository<Infrasturcture.Entities.User> userRepository)
        {
            _userRepository = userRepository;
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

            return new ApplicationResult { Success = true };
        }
    }
}

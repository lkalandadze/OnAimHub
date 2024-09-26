using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Helpers.Password;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ForgotPassword;

public class ResetPasswordHandler : ICommandHandler<ResetPassword, ApplicationResult>
{
    private readonly IRepository<User> _userRepository;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public ResetPasswordHandler(
        IRepository<User> userRepository,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _userRepository = userRepository;
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
            throw new Exception("Invalid or expired reset token.");

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

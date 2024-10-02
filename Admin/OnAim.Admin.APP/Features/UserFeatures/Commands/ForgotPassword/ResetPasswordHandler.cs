using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Helpers.Password;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ForgotPassword;

public class ResetPasswordHandler : BaseCommandHandler<ResetPassword, ApplicationResult>
{
    private readonly IRepository<User> _userRepository;

    public ResetPasswordHandler(
        CommandContext<ResetPassword> Context,
        IRepository<User> userRepository
        ) : base( Context )
    {
        _userRepository = userRepository;
    }

    protected override async Task<ApplicationResult> ExecuteAsync(ResetPassword request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var user = await _userRepository.Query(x =>
             x.Email == request.Email &&
             x.ResetCode == request.Code &&
             x.ResetCodeExpiration > DateTime.UtcNow &&
             !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            throw new BadRequestException("Invalid or expired reset token.");

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

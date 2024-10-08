using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.APP.Services.AuthServices;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ChangePassword;

public class ChangePasswordCommandHandler : BaseCommandHandler<ChangePasswordCommand, ApplicationResult>
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordService _passwordService;

    public ChangePasswordCommandHandler(
        CommandContext<ChangePasswordCommand> context,
        IRepository<User> userRepository,
        IPasswordService passwordService
        ) : base( context )
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var user = await _userRepository.Query(x => x.Email == request.Email).FirstOrDefaultAsync();

        if (user == null)
            throw new NotFoundException("User Not Found!");

        if (user?.IsActive == false)
            throw new NotFoundException("User Not Found!");

        string hashedOldPassword = _passwordService.EncryptPassword(request.OldPassword, user.Salt);

        if (user.Password != hashedOldPassword)
            throw new BadRequestException("Old password is incorrect!");

        var newSalt = _passwordService.Salt();
        string hashedNewPassword = _passwordService.EncryptPassword(request.NewPassword, newSalt);

        user.Password = hashedNewPassword;
        user.Salt = newSalt;
        user.DateUpdated = SystemDate.Now;

        try
        {
            await _userRepository.CommitChanges();
        }
        catch (Exception ex)
        {
            throw new BadRequestException("An error occurred while updating the password.");
        }

        return new ApplicationResult
        {
            Success = true,
        };
    }
}

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.Helpers.Password;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ChangePassword;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, ApplicationResult>
{
    private readonly IRepository<User> _userRepository;
    private readonly IValidator<ChangePasswordCommand> _validator;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public ChangePasswordCommandHandler(
        IRepository<User> userRepository,
        IValidator<ChangePasswordCommand> validator,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _userRepository = userRepository;
        _validator = validator;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var user = await _userRepository.Query(x => x.Email == request.Email).FirstOrDefaultAsync();

        if (user == null)
            throw new NotFoundException("User Not Found!");

        if (user?.IsActive == false)
            throw new NotFoundException("User Not Found!");

        string hashedOldPassword = EncryptPasswordExtension.EncryptPassword(request.OldPassword, user.Salt);

        if (user.Password != hashedOldPassword)
            throw new BadRequestException("Old password is incorrect!");

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

        return new ApplicationResult
        {
            Success = true,
        };
    }
}

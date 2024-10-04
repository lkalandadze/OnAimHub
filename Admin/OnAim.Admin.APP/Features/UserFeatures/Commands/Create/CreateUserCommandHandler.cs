using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Helpers.Password;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Create;

public class CreateUserCommandHandler : BaseCommandHandler<CreateUserCommand, ApplicationResult>
{
    private readonly IRepository<Domain.Entities.User> _repository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IConfigurationRepository<UserRole> _configurationRepository;
    private readonly IEmailService _emailService;

    public CreateUserCommandHandler(
        CommandContext<CreateUserCommand> context,
        IRepository<Domain.Entities.User> repository,
        IRepository<Role> roleRepository,
        IConfigurationRepository<UserRole> configurationRepository,
        IEmailService emailService
        ) : base( context )
    {
        _repository = repository;
        _roleRepository = roleRepository;
        _configurationRepository = configurationRepository;
        _emailService = emailService;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var existingUser = await _repository.Query(x => x.Email == request.Email && !x.IsDeleted).FirstOrDefaultAsync();

        if (existingUser != null)
            throw new BadRequestException($"User creation failed. A user already exists with email: {request.Email}");

        if (existingUser?.IsActive == false)
            throw new BadRequestException($"User creation failed. A user already exists with email: {request.Email}");

        if (existingUser != null)
        {
            if(existingUser.IsDeleted == true)
            {
                await CreateUserWithTemporaryPassword(request);
            }
            else
            {
                HandleExistingUser(existingUser);
            }
        }
        else
        {
            await CreateUserWithTemporaryPassword(request);
        }

        return new ApplicationResult { Success = true };
    }

    private async Task<User> CreateUserWithTemporaryPassword(CreateUserCommand request)
    {
        var temporaryPassword = PasswordHelper.GenerateTemporaryPassword();
        var salt = EncryptPasswordExtension.Salt();
        string hashed = EncryptPasswordExtension.EncryptPassword(temporaryPassword, salt);

        var user = CreateUser(
            request.FirstName,
            request.LastName,
            request.Email,
            hashed,
            salt,
            request.Phone,
            true,
            true,
            false 
        );

        await _repository.Store(user);
        await _repository.CommitChanges();

        var role = await _roleRepository.Query(x => x.Name == "DefaultRole").FirstOrDefaultAsync();
        await AssignRoleToUserAsync(user.Id, user, role.Id, role);

        string htmlBody = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  " +
            "  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n   " +
            " <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\r\n    <title>Email</title>\r\n    <style>\r\n   " +
            "     @media (prefers-color-scheme: dark) {\r\n         " +
            "   .background {\r\n                background-color: #EDEFF2 !important;\r\n      " +
            "      }\r\n\r\n            .container {\r\n                background-color: #ffffff !important;\r\n            }\r\n\r\n    " +
            "        .password-box {\r\n                background-color: rgba(0, 167, 111, 0.08) !important;\r\n            }\r\n        }\r\n " +
            "   </style>\r\n</head>\r\n<body style=\"background-color: #EDEFF2; margin: 0; padding: 0; color: #000000; font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;\">\r\n    <div class=\"background\" style=\"max-width: 558px; width: 100%; height: 80px; margin: 48px auto;\">\r\n        <img src=\"cid:logoImage\" alt=\"logo\" />\r\n    </div>\r\n\r\n    <div class=\"container\" style=\"max-width: 558px; width: 100%; padding: 48px; background-color: white; margin: auto;\">\r\n      " +
            "  <p style=\"font-size: 16px; font-weight: 600; line-height: 21.79px; color: rgba(28, 37, 46, 1);\">Dear, {firstName}</p>\r\n        <p style=\"font-size: 16px; font-weight: 600; line-height: 21.79px; color: rgba(28, 37, 46, 1);\">Your Account Has Been Successfully Activated!</p>\r\n        <p style=\"font-size: 14px; font-weight: 400; line-height: 20px; color: rgba(28, 37, 46, 1);\">Here is the temporary password you need to access your account!</p>\r\n        <p style=\"font-size: 14px; font-weight: 400; line-height: 20px; color: rgba(28, 37, 46, 1);\">For safety reasons, we recommend changing your password.</p>\r\n\r\n    " +
            "    <div class=\"password-box\" style=\"max-width: 366px; width: 100%; height: 124px; background-color: rgba(0, 167, 111, 0.08); margin: 48px auto 240px; padding: 24px 111px; text-align: center;\">\r\n            <p style=\"color: #637381; font-size: 12px; font-weight: 600; line-height: 16.34px; margin: 0;\">Temporary password</p>\r\n            <p style=\"color: rgba(0, 167, 111, 1); font-size: 32px; font-weight: 800; line-height: 43.58px; margin: 16px 0 0;\">{temporaryPassword}</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n";

        htmlBody = htmlBody.Replace("{firstName}", user.FirstName)
                           .Replace("{temporaryPassword}", temporaryPassword);

        await _emailService.SendActivationEmailAsync(user.Email, "Account Created", htmlBody);

        return user;
    }

    private User CreateUser(
        string firstName,
        string lastName,
        string email,
        string hashed,
        string salt,
        string phone,
        bool isVerified,
        bool isActive,
        bool isSuperAdmin)
    {
        return new Domain.Entities.User(
            firstName,
            lastName,
            email,
            email,
            hashed,
            salt,
            phone,
            _context.SecurityContextAccessor.UserId,
            isVerified,
            isActive,
            null,
            null,
            isSuperAdmin
        );
    }

    private void HandleExistingUser(User existingUser)
    {
        if (existingUser.IsActive == false)
        {
            throw new BadRequestException($"User creation failed. The user with email: {existingUser.Email} is inactive.");
        }
        else
        {
            throw new BadRequestException($"User creation failed. A user already exists with email: {existingUser.Email}");
        }
    }

    private async Task AssignRoleToUserAsync(int userId, Domain.Entities.User user, int roleId, Role role)
    {
        var userRole = new UserRole(userId, roleId);
        await _configurationRepository.Store(userRole);
        await _configurationRepository.CommitChanges();
    }
}

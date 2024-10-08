using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Helpers;
using OnAim.Admin.Shared.Helpers.Password;
using OnAim.Admin.Shared.Enums;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Registration;

public class RegistrationCommandHandler : BaseCommandHandler<RegistrationCommand, ApplicationResult>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IConfigurationRepository<UserRole> _configurationRepository;
    private readonly IEmailService _emailService;
    private readonly IDomainValidationService _domainValidationService;

    public RegistrationCommandHandler(
        CommandContext<RegistrationCommand> context,
        IRepository<User> userRepository,
        IRepository<Role> roleRepository,
        IConfigurationRepository<UserRole> configurationRepository,
        IEmailService emailService,
        IDomainValidationService domainValidationService
        ) : base(context)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _configurationRepository = configurationRepository;
        _emailService = emailService;
        _domainValidationService = domainValidationService;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(RegistrationCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var existingUser = await GetExistingUserAsync(request.Email);

        if (!await _domainValidationService.IsDomainAllowedAsync(request.Email))
            throw new BadRequestException("Domain not allowed");

        if (existingUser != null)
        {
            if (existingUser.IsDeleted)
            {
                await CreateNewUser(request);
            }
            else
            {
                HandleExistingUser(existingUser);
            }
        }
        else
        {
            await CreateNewUser(request);
        }

        return new ApplicationResult { Success = true };
    }

    private async Task<User?> GetExistingUserAsync(string email)
    {
        return await _userRepository.Query(x => x.Email == email).FirstOrDefaultAsync();
    }

    private async Task AssignDefaultRoleToUserAsync(User user)
    {
        var role = await _roleRepository.Query(x => x.Name == "DefaultRole").FirstOrDefaultAsync();
        if (role != null)
        {
            await AssignRoleToUserAsync(user.Id, role.Id);
        }
    }

    private async Task AssignRoleToUserAsync(int userId, int roleId)
    {
        var userRole = new UserRole(userId, roleId);
        await _configurationRepository.Store(userRole);
        await _configurationRepository.CommitChanges();
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

    private async Task CreateNewUser(RegistrationCommand request)
    {
        var salt = EncryptPasswordExtension.Salt();
        string hashed = EncryptPasswordExtension.EncryptPassword(request.Password, salt);

        var activationCode = ActivationCodeHelper.ActivationCode().ToString();
        var activationCodeExpiration = DateTime.UtcNow.AddMinutes(15);

        var user = new User(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Email,
            hashed,
            salt,
            request.Phone,
            _context.SecurityContextAccessor.UserId,
            isVerified: false,
            isActive: false,
            activationCode,
            VerificationPurpose.AccountActivation,
            activationCodeExpiration,
            isSuperAdmin: false
        );

        await _userRepository.Store(user);
        await _userRepository.CommitChanges();

        var role = await _roleRepository.Query(x => x.Name == "DefaultRole").FirstOrDefaultAsync();
        await AssignRoleToUserAsync(user.Id, role.Id);

        await SendActivationEmailAsync(user, activationCode);
    }

    private async Task SendActivationEmailAsync(User user, string activationCode)
    {
        var activationLink = $"activate?userId={user.Id}&code={activationCode}";

        string htmlBody = "" +
            "<!DOCTYPE html>\n<html>\n<head>\n    " +
            "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    " +
            "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n   " +
            " <title>Verify Your Account</title>\n</head>\n<body style=\"font-family: Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 20px;\">\n    " +
            "<h1>Account Verification</h1>\n    <p>Hi {firstName},</p>\n    <p>Thank you for registering. Please click the link below to verify your email address:</p>\n   " +
            " <p style=\"color: rgba(0, 167, 111, 1); font-size: 32px; font-weight: 800; line-height: 43.58px; margin: 16px 0 0;\">{code}</p>\n    <p><a href=\"{link}\">Verify your account</a></p>\n   " +
            " <p>If you didn't request this, please ignore this email.</p>\n    <p>Best regards,<br>Your Company</p>\n</body>\n</html>\n";

        htmlBody = htmlBody.Replace("{firstName}", user.FirstName)
                           .Replace("{code}", activationCode.ToString())
                           .Replace("{link}", activationLink);

        await _emailService.SendActivationEmailAsync(user.Email, "Activation Code", htmlBody);
    }
}

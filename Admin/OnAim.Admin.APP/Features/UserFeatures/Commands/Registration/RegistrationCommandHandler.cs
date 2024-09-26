using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.Shared.Helpers;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.Helpers.Password;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Registration;

public class RegistrationCommandHandler : ICommandHandler<RegistrationCommand, ApplicationResult>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IConfigurationRepository<UserRole> _configurationRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<RegistrationCommand> _logger;
    private readonly IValidator<RegistrationCommand> _validator;
    private readonly IDomainValidationService _domainValidationService;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public RegistrationCommandHandler(
        IRepository<User> userRepository,
        IRepository<Role> roleRepository,
        IConfigurationRepository<UserRole> configurationRepository,
        IEmailService emailService,
        ILogger<RegistrationCommand> logger,
        IValidator<RegistrationCommand> validator,
        IDomainValidationService domainValidationService,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _configurationRepository = configurationRepository;
        _emailService = emailService;
        _logger = logger;
        _validator = validator;
        _domainValidationService = domainValidationService;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(RegistrationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);          

        var existingUser = await _userRepository.Query(x => x.Email == request.Email && !x.IsDeleted).FirstOrDefaultAsync();

        if (existingUser != null)
            throw new AlreadyExistsException($"User creation failed. A user already exists with email: {request.Email}");           

        if (existingUser?.IsActive == false)
            throw new AlreadyExistsException($"User creation failed. A user already exists or is not active with email: {request.Email}");          

        if (!await _domainValidationService.IsDomainAllowedAsync(request.Email))
            throw new BadRequestException("Domain not allowed");           

        var salt = EncryptPasswordExtension.Salt();
        string hashed = EncryptPasswordExtension.EncryptPassword(request.Password, salt);

        var activationCode = ActivationCodeHelper.ActivationCode();
        var ActivationCodeExpiration = DateTime.UtcNow.AddMinutes(15);

        var localhostBaseUrl = "https://localhost:7126";
        var verificationUrl = $"{localhostBaseUrl}/verify?code={activationCode}";

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Email,
            Email = request.Email,
            Salt = salt,
            Password = hashed,
            Phone = request.Phone,
            DateOfBirth = request.DateOfBirth,
            DateCreated = SystemDate.Now,
            IsActive = true,
            CreatedBy = _securityContextAccessor.UserId,
            ActivationCode = activationCode,
            ActivationCodeExpiration = ActivationCodeExpiration,
            IsVerified = false
        };

        _logger.LogInformation("Creating user with email: {Email}", request.Email);
        await _userRepository.Store(user);
        await _userRepository.CommitChanges();

        var role = await _roleRepository.Query(x => x.Name == "DefaultRole").FirstOrDefaultAsync();
        await AssignRoleToUserAsync(user.Id, role.Id);

        var activationLink = $"activate?userId={user.Id}&code={activationCode}";

        string htmlBody = "" +
            "<!DOCTYPE html>\n<html>\n<head>\n    " +
            "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    " +
            "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n   " +
            " <title>Verify Your Account</title>\n</head>\n<body style=\"font-family: Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 20px;\">\n    " +
            "<h1>Account Verification</h1>\n    <p>Hi {firstName},</p>\n    <p>Thank you for registering. Please click the link below to verify your email address:</p>\n   " +
            " <p style=\"color: rgba(0, 167, 111, 1); font-size: 32px; font-weight: 800; line-height: 43.58px; margin: 16px 0 0;\">{code}</p>\n    <p><a href=\"{link}\">Verify your account</a></p>\n   " +
            " <p>If you didn't request this, please ignore this email.</p>\n    <p>Best regards,<br>Your Company</p>\n</body>\n</html>\n";

        //string templatePath = Path.Combine("Templates", "Emails", "Register.html");
        //string htmlBody = await ReadEmailTemplateAsync(templatePath);

        htmlBody = htmlBody.Replace("{firstName}", user.FirstName)
                           .Replace("{code}", activationCode.ToString())
                           .Replace("{link}", activationLink);

        await _emailService.SendActivationEmailAsync(user.Email, "Activation Code", htmlBody);


        return new ApplicationResult
        {
            Success = true,
        };
    }

    private async Task<string> ReadEmailTemplateAsync(string path)
    {
        return await File.ReadAllTextAsync(path);
    }

    private async Task AssignRoleToUserAsync(int userId, int roleId)
    {
        var userRole = new UserRole { UserId = userId, RoleId = roleId };
        await _configurationRepository.Store(userRole);
        await _configurationRepository.CommitChanges();

        _logger.LogInformation("Role assigned successfully.");
    }
}

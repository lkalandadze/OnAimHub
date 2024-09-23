using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.Exceptions;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.Shared.Helpers;

namespace OnAim.Admin.APP.Commands.User.Create
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, ApplicationResult>
    {
        private readonly IValidator<CreateUserCommand> _validator;
        private readonly IRepository<Infrasturcture.Entities.User> _repository;
        private readonly IRepository<Infrasturcture.Entities.Role> _roleRepository;
        private readonly IConfigurationRepository<UserRole> _configurationRepository;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly IEmailService _emailService;
        private readonly IAuditLogService _auditLogService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public CreateUserCommandHandler(
            IValidator<CreateUserCommand> validator,
            IRepository<Infrasturcture.Entities.User> repository,
            IRepository<Infrasturcture.Entities.Role> roleRepository,
            IConfigurationRepository<UserRole> configurationRepository,
            ILogger<CreateUserCommandHandler> logger,
            IEmailService emailService,
            IAuditLogService auditLogService,
            ISecurityContextAccessor securityContextAccessor
            )
        {
            _validator = validator;
            _repository = repository;
            _roleRepository = roleRepository;
            _configurationRepository = configurationRepository;
            _logger = logger;
            _emailService = emailService;
            _auditLogService = auditLogService;
            _securityContextAccessor = securityContextAccessor;
        }
        public async Task<ApplicationResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateUserCommand. Errors: {Errors}", validationResult.Errors);

                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var existingUser = await _repository.Query(x => x.Email == request.Email && !x.IsDeleted).FirstOrDefaultAsync();

            if (existingUser != null || existingUser.IsActive == false)
            {
                _logger.LogError("User creation failed. A user already exists with email: {Email}", request.Email);

                throw new AlreadyExistsException($"User creation failed. A user already exists with email: {request.Email}");
            }

            if (existingUser.IsActive == false)
            {
                _logger.LogError("User creation failed. A user already exists with email: {Email}", request.Email);

                throw new AlreadyExistsException($"User creation failed. A user already exists with email: {request.Email}");
            }

            var temporaryPassword = PasswordHelper.GenerateTemporaryPassword();
            var salt = EncryptPasswordExtension.Salt();
            string hashed = EncryptPasswordExtension.EncryptPassword(temporaryPassword, salt);

            var userId = _securityContextAccessor.UserId;

            var activationToken = Guid.NewGuid().ToString();
            var tokenExpiration = DateTime.UtcNow.AddHours(24);

            var user = new Infrasturcture.Entities.User
            {
                FirstName = request.FirstName.ToLower(),
                LastName = request.LastName.ToLower(),
                Username = request.Email,
                Email = request.Email,
                Salt = salt,
                Password = hashed,
                Phone = request.Phone,
                DateOfBirth = request.DateOfBirth,
                DateCreated = SystemDate.Now,
                IsActive = true,
                IsVerified = true,
                CreatedBy = userId,
            };

            _logger.LogInformation("Creating user with email: {Email}", request.Email);
            await _repository.Store(user);
            await _repository.CommitChanges();

            var role = await _roleRepository.Query(x => x.Name == "DefaultRole").FirstOrDefaultAsync();
            await AssignRoleToUserAsync(user.Id, role.Id);

            _logger.LogInformation("User created successfully with ID: {UserId} by User ID: {CurrentUserId}", user.Id, _securityContextAccessor.UserId);

            //string htmlBody = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\r\n    <title>Email</title>\r\n    <style>\r\n        @media (prefers-color-scheme: dark) {\r\n            .background {\r\n                background-color: #EDEFF2 !important;\r\n            }\r\n\r\n            .container {\r\n                background-color: #ffffff !important;\r\n            }\r\n\r\n            .password-box {\r\n                background-color: rgba(0, 167, 111, 0.08) !important;\r\n            }\r\n        }\r\n    </style>\r\n</head>\r\n<body style=\"background-color: #EDEFF2; margin: 0; padding: 0; color: #000000; font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;\">\r\n    <div class=\"background\" style=\"max-width: 558px; width: 100%; height: 80px; margin: 48px auto;\">\r\n        <img src=\"cid:logoImage\" alt=\"logo\" />\r\n    </div>\r\n\r\n    <div class=\"container\" style=\"max-width: 558px; width: 100%; padding: 48px; background-color: white; margin: auto;\">\r\n        <p style=\"font-size: 16px; font-weight: 600; line-height: 21.79px; color: rgba(28, 37, 46, 1);\">Dear, {firstName}</p>\r\n        <p style=\"font-size: 16px; font-weight: 600; line-height: 21.79px; color: rgba(28, 37, 46, 1);\">Your Account Has Been Successfully Activated!</p>\r\n        <p style=\"font-size: 14px; font-weight: 400; line-height: 20px; color: rgba(28, 37, 46, 1);\">Here is the temporary password you need to access your account!</p>\r\n        <p style=\"font-size: 14px; font-weight: 400; line-height: 20px; color: rgba(28, 37, 46, 1);\">For safety reasons, we recommend changing your password.</p>\r\n\r\n        <div class=\"password-box\" style=\"max-width: 366px; width: 100%; height: 124px; background-color: rgba(0, 167, 111, 0.08); margin: 48px auto 240px; padding: 24px 111px; text-align: center;\">\r\n            <p style=\"color: #637381; font-size: 12px; font-weight: 600; line-height: 16.34px; margin: 0;\">Temporary password</p>\r\n            <p style=\"color: rgba(0, 167, 111, 1); font-size: 32px; font-weight: 800; line-height: 43.58px; margin: 16px 0 0;\">{temporaryPassword}</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n";

            string templatePath = Path.Combine("Templates", "Emails", "CreateUserEmail.html");
            string htmlBody = await ReadEmailTemplateAsync(templatePath);

            htmlBody = htmlBody.Replace("{firstName}", user.FirstName)
                               .Replace("{temporaryPassword}", temporaryPassword);

            await _emailService.SendActivationEmailAsync(user.Email, "Account Created", htmlBody);

            var auditLog = new AuditLog
            {
                UserId = _securityContextAccessor.UserId,
                Timestamp = SystemDate.Now,
                Action = "CREATE",
                ObjectId = user.Id,
                Category = "User",
                Log = $"User Created successfully with ID: {user.Id} by User ID: {_securityContextAccessor.UserId}"
            };

            await _auditLogService.LogEventAsync(auditLog);

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
}

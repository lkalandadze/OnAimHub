using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Exceptions;
using OnAim.Admin.APP.Helpers;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

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

            var existingUser = await _repository.Query(x => x.Email == request.Email).FirstOrDefaultAsync();

            if (existingUser != null)
            {
                _logger.LogError("User creation failed. A user already exists with email: {Email}", request.Email);

                throw new AlreadyExistsException($"User creation failed. A user already exists with email: {request.Email}");
            }

            var temporaryPassword = PasswordHelper.GenerateTemporaryPassword();
            var salt = Infrasturcture.Extensions.EncryptPasswordExtension.Salt();
            string hashed = Infrasturcture.Extensions.EncryptPasswordExtension.EncryptPassword(temporaryPassword, salt);

            var userId = _securityContextAccessor.UserId;

            var activationToken = Guid.NewGuid().ToString();
            var tokenExpiration = DateTime.UtcNow.AddHours(24);

            var user = new Infrasturcture.Entities.User
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
                UserId = userId,
                //ActivationToken = activationToken,
                //ActivationTokenExpiration = tokenExpiration
            };

            _logger.LogInformation("Creating user with email: {Email}", request.Email);
            await _repository.Store(user);
            await _repository.CommitChanges();

            var role = await _roleRepository.Query(x => x.Name == "DefaultRole").FirstOrDefaultAsync();
            await AssignRoleToUserAsync(user.Id, role.Id);

            _logger.LogInformation("User created successfully with ID: {UserId} by User ID: {CurrentUserId}", user.Id, _securityContextAccessor.UserId);

            await _emailService.SendActivationEmailAsync(
               user.Email,
               "Your Account is Created",
               temporaryPassword,
               user.FirstName
           );

            await _auditLogService.LogEventAsync(
               SystemDate.Now,
               "Create",
               nameof(User),
               user.Id,
               _securityContextAccessor.UserId,
               $"User Created successfully with ID: {user.Id} by User ID: {_securityContextAccessor.UserId}");

            return new ApplicationResult
            {
                Success = true,
            };
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

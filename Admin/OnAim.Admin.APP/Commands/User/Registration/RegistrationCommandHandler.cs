using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Exceptions;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.User.Registration
{
    public class RegistrationCommandHandler : ICommandHandler<RegistrationCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _userRepository;
        private readonly IRepository<Infrasturcture.Entities.Role> _roleRepository;
        private readonly IConfigurationRepository<UserRole> _configurationRepository;
        private readonly IEmailService _emailService;
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<RegistrationCommand> _logger;
        private readonly IValidator<RegistrationCommand> _validator;
        private readonly IDomainValidationService _domainValidationService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public RegistrationCommandHandler(
            IRepository<Infrasturcture.Entities.User> userRepository,
            IRepository<Infrasturcture.Entities.Role> roleRepository,
            IConfigurationRepository<UserRole> configurationRepository,
            IEmailService emailService,
            IAuditLogService auditLogService,
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
            _auditLogService = auditLogService;
            _logger = logger;
            _validator = validator;
            _domainValidationService = domainValidationService;
            _securityContextAccessor = securityContextAccessor;
        }
        public async Task<ApplicationResult> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateUserCommand. Errors: {Errors}", validationResult.Errors);

                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var existingUser = await _userRepository.Query(x => x.Email == request.Email).FirstOrDefaultAsync();

            if (existingUser != null)
            {
                _logger.LogError("User creation failed. A user already exists with email: {Email}", request.Email);

                throw new AlreadyExistsException($"User creation failed. A user already exists with email: {request.Email}");
            }

            if (!await _domainValidationService.IsDomainAllowedAsync(request.Email))
            {
                throw new BadRequestException("Domain not allowed");
            }

            var salt = Infrasturcture.Extensions.EncryptPasswordExtension.Salt();
            string hashed = Infrasturcture.Extensions.EncryptPasswordExtension.EncryptPassword(request.Password, salt);

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
            await _userRepository.Store(user);
            await _userRepository.CommitChanges();

            var role = await _roleRepository.Query(x => x.Name == "DefaultRole").FirstOrDefaultAsync();
            await AssignRoleToUserAsync(user.Id, role.Id);

            _logger.LogInformation("User created successfully with ID: {UserId} by User ID: {CurrentUserId}", user.Id, _securityContextAccessor.UserId);

            await _auditLogService.LogEventAsync(
               SystemDate.Now,
               "Registration",
               nameof(User),
               user.Id,
               _securityContextAccessor.UserId,
               $"User Created successfully with ID: {user.Id}");

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

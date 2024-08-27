using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnAim.Admin.APP.Extensions;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using System.Security.Claims;
using static OnAim.Admin.APP.Extensions.Extension;

namespace OnAim.Admin.APP.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApplicationResult>
    {
        private readonly IValidator<CreateUserCommand> _validator;
        private readonly IRepository<Infrasturcture.Entities.User> _repository;
        private readonly IRepository<Infrasturcture.Entities.Role> _roleRepository;
        private readonly IConfigurationRepository<UserRole> _configurationRepository;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(
            IValidator<CreateUserCommand> validator,
            IRepository<Infrasturcture.Entities.User> repository,
            IRepository<Infrasturcture.Entities.Role> roleRepository,
            IConfigurationRepository<UserRole> configurationRepository,
            ILogger<CreateUserCommandHandler> logger
            )
        {
            _validator = validator;
            _repository = repository;
            _roleRepository = roleRepository;
            _configurationRepository = configurationRepository;
            _logger = logger;
        }
        public async Task<ApplicationResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateUserCommand. Errors: {Errors}", validationResult.Errors);

                return new ApplicationResult
                {
                    Success = false,
                    Data = validationResult.Errors,
                };
            }

            var existingUser = await _repository.Query(x => x.Email == request.Email).FirstOrDefaultAsync();

            if (existingUser != null)
            {
                _logger.LogError("User creation failed. A user already exists with email: {Email}", request.Email);

                return new ApplicationResult { Success = false, Data = validationResult.Errors };
            }

            var salt = Infrasturcture.Extensions.EncryptPasswordExtension.Salt();
            string hashed = Infrasturcture.Extensions.EncryptPasswordExtension.EncryptPassword(request.Password, salt);

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
                UserId = HttpContextAccessorProvider.HttpContextAccessor.GetUserId()
            };

            _logger.LogInformation("Creating user with email: {Email}", request.Email);
            await _repository.Store(user);
            await _repository.CommitChanges();

            var role = await _roleRepository.Query(x => x.Name == "DefaultRole").FirstOrDefaultAsync();
            await AssignRoleToUserAsync(user.Id, role.Id);

            _logger.LogInformation("User created successfully with ID: {UserId} by User ID: {CurrentUserId}", user.Id, HttpContextAccessorProvider.HttpContextAccessor.GetUserId());

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

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using System.Security.Cryptography;

namespace OnAim.Admin.APP.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApplicationResult>
    {
        private readonly IValidator<CreateUserCommand> _validator;
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(
            IValidator<CreateUserCommand> validator,
            IUserRepository userRepository
            )
        {
            _validator = validator;
            _userRepository = userRepository;
        }
        public async Task<ApplicationResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new ApplicationResult
                {
                    Success = false,
                    Data = validationResult.Errors,
                };
            }

            var existingUser = await _userRepository.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new Exception("User Already Exists With This Email");
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
                IsActive = true
            };

            var result = await _userRepository.Create(user);

            return new ApplicationResult
            {
                Success = true,
                Data = result.Email,
                Errors = null
            };
        }
    }
}

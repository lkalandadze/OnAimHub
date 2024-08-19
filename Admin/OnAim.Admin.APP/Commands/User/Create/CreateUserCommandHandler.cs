using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using System.Data;
using System.Security.Cryptography;

namespace OnAim.Admin.APP.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApplicationResult>
    {
        private readonly IValidator<CreateUserCommand> _validator;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public CreateUserCommandHandler(
            IValidator<CreateUserCommand> validator,
            IUserRepository userRepository,
            IRoleRepository roleRepository
            )
        {
            _validator = validator;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public async Task<ApplicationResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ApplicationResult
                {
                    Success = false,
                    Errors = null
                };
            }

            var existingUser = await _userRepository.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new Exception("User Already Exists With This Email");
            }

            var salt = Salt();
            string hashed = EncryptPassword(request.Password, salt);

            var user = new Infrasturcture.Entities.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Salt = salt,
                Password = hashed,
                Phone = request.Phone,
                UserId = request.UserId,
                DateOfBirth = request.DateOfBirth,
                DateCreated = SystemDate.Now,
            };

            var result = await _userRepository.Create(user);

            //if (request.Roles != null && request.Roles.Any())
            //{
            //    var roleIds = request.Roles.Select(r => r.Id).ToList();
            //    var roles = await _roleRepository.GetRolesByIdsAsync(roleIds);
            //    foreach (var role in roles)
            //    {
            //        await _roleRepository.AssignRoleToUserAsync(role.UserId, role.Id);
            //    }
            //}
            return new ApplicationResult
            {
                Success = true,
                Data = result,
                Errors = null
            };
        }

        private string EncryptPassword(string password, string salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                         password: password,
                                                         salt: Convert.FromBase64String(salt),
                                                         prf: KeyDerivationPrf.HMACSHA256,
                                                         iterationCount: 100000,
                                                         numBytesRequested: 256 / 8));
        }

        private string Salt()
        {
            byte[] salt = new byte[128 / 8];

            RandomNumberGenerator.Fill(salt);

            return Convert.ToBase64String(salt);
        }
    }
}

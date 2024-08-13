using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using OnAim.Admin.APP.Models.Response.User;
using OnAim.Admin.Infrasturcture.Configuration;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using System.Security.Claims;
using System.Security.Cryptography;

namespace OnAim.Admin.APP.Commands.User.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResultDto>
    {
        private readonly UserManager<Infrasturcture.Entities.User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IJwtFactory _jwtFactory;
        private readonly IValidator<LoginUserCommand> _validator;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IJwtFactory jwtFactory,
            IValidator<LoginUserCommand> validator
            )
        {
            _userRepository = userRepository;
            _jwtFactory = jwtFactory;
            _validator = validator;
        }
        public async Task<AuthResultDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var model = request.Model;
            var user = await _userRepository.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new Exception("Invalid credentials.");
            }

            string hashed = EncryptPassword(model.Password, user.Salt);

            if (hashed != user.Password)
            {
                throw new Exception("Invalid credentials.");
            }
            var roles = await _userRepository.GetUserRolesAsync(user.Id);
            var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);

            var roleNames = roles.Select(r => r.Name).ToList();
            var token = _jwtFactory.GenerateEncodedToken(user.Id, user.Email, new List<Claim>(), roleNames, permissions);

            return new AuthResultDto
            {
                Token = token,
                StatusCode = 200
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

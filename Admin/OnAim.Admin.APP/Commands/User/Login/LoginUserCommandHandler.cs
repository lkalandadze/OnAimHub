using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using OnAim.Admin.APP.Models.Response.User;
using OnAim.Admin.Identity.Services;
using OnAim.Admin.Infrasturcture.Configuration;
using OnAim.Admin.Infrasturcture.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.Models;
using System.Security.Claims;
using System.Security.Cryptography;

namespace OnAim.Admin.APP.Commands.User.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResultDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtFactory _jwtFactory;
        private readonly IValidator<LoginUserCommand> _validator;
        private readonly ApplicationUserManager _userManager;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IJwtFactory jwtFactory,
            IValidator<LoginUserCommand> validator,
            ApplicationUserManager userManager
            )
        {
            _userRepository = userRepository;
            _jwtFactory = jwtFactory;
            _validator = validator;
            _userManager = userManager;
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

            if (!user.IsActive || user == null)
            {
                throw new UserNotFoundException("User Not Found");
            }

            string hashed = EncryptPassword(model.Password, user.Salt);

            if (hashed == user.Password)
            {
                var roles = await _userRepository.GetUserRolesAsync(user.Id);
                var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);

                var roleNames = roles.Select(r => r.Name).ToList();
                var token = _jwtFactory.GenerateEncodedToken(user.Id, user.Email, new List<Claim>(), roleNames);

                var identityUser = await _userManager.FindByNameAsync(user.Username);
                if (identityUser == null)
                {
                    try
                    {
                        var idp = await _userManager.CreateAsync(new Identity.Entities.User
                        {
                            Email = user.Email,
                            UserName = user.Username,
                            PhoneNumber = user.Phone,
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true,
                            SecurityStamp = Guid.NewGuid().ToString("D"),
                            CreateDate = SystemDate.Now,
                        }, request.Model.Password);

                        if (idp.Succeeded)
                        {
                            identityUser = await _userManager.FindByNameAsync(user.Username);

                            foreach (var item in roles)
                            {
                                await _userManager.AddToRoleAsync(identityUser, item.Name);
                                await _userManager.AddClaimAsync(identityUser, new Claim(ClaimTypes.Name, item.Name));
                            }
                        }
                    }
                    catch { }
                }

                return new AuthResultDto
                {
                    Token = token,
                    StatusCode = 200
                };
            }

            return new AuthResultDto
            {
                Token = null,
                StatusCode = 404
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

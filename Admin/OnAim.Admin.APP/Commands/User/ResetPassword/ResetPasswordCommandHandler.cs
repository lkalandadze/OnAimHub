using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using OnAim.Admin.Identity.Services;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.Security.Cryptography;

namespace OnAim.Admin.APP.Commands.User.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ApplicationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationUserManager _userManager;
        private readonly IValidator<ResetPasswordCommand> _validator;

        public ResetPasswordCommandHandler(
            IUserRepository userRepository,
            ApplicationUserManager userManager,
            IValidator<ResetPasswordCommand> validator
            )
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _validator = validator;
        }

        public async Task<ApplicationResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
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

            await _userRepository.ResetPassword(request.Id, request.Password);

            //if (user != null && user.IsActive)
            //{
            //    var salt = Salt();

            //    string hashed = EncryptPassword(request.Password, salt);

            //    user.Password = hashed;
            //    user.Salt = salt;

            //    await _userRepository.CommitChanges();

            //    //var identityUser = await _userManager.FindByEmailAsync(user.Email);
            //    //if (identityUser != null)
            //    //{
            //    //    var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
            //    //    var identityResult = await _userManager.ResetPasswordAsync(identityUser, passwordResetToken, request.Password);
            //    //    if (identityResult.Succeeded)
            //    //    {
            //    //        await _userRepository.CommitChanges();
            //    //    }
            //    //    else
            //    //    {
            //    //        var error = identityResult.Errors.FirstOrDefault();
            //    //        await Fail(new Error
            //    //        {
            //    //            Code = StatusCodes.Status400BadRequest,
            //    //            Message = error?.Description ?? string.Empty
            //    //        });
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    await _userRepository.CommitChanges();
            //    //}
            //}

            return new ApplicationResult
            {
                Success = true,
            };
        }

        private async Task Fail(Error error)
        {
            throw new NotImplementedException();
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

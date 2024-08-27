using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Extensions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.Security.Cryptography;

namespace OnAim.Admin.APP.Commands.User.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _userRepository;
        private readonly IValidator<ResetPasswordCommand> _validator;

        public ResetPasswordCommandHandler(
            IRepository<Infrasturcture.Entities.User> userRepository,
            IValidator<ResetPasswordCommand> validator
            )
        {
            _userRepository = userRepository;
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

            var user = await _userRepository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (user != null && user.IsActive)
            {
                var salt = EncryptPasswordExtension.Salt();

                string hashed = EncryptPasswordExtension.EncryptPassword(request.Password, salt);

                user.Password = hashed;
                user.Salt = salt;

                await _userRepository.CommitChanges();
            }

            return new ApplicationResult
            {
                Success = true,
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

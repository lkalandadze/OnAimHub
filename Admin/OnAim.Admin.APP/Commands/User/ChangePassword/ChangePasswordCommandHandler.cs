using FluentValidation;
using MediatR;
using OnAim.Admin.Identity.Services;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ApplicationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<ChangePasswordCommand> _validator;
        private readonly ApplicationUserManager _userManager;

        public ChangePasswordCommandHandler(
            IUserRepository userRepository,
            IValidator<ChangePasswordCommand> validator,
            ApplicationUserManager userManager
            )
        {
            _userRepository = userRepository;
            _validator = validator;
            _userManager = userManager;
        }
        public async Task<ApplicationResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
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

            var user = await _userRepository.FindByEmailAsync(request.Email);

            if (user == null || !user.IsActive)
            {
                return new ApplicationResult
                {
                    Success = false,
                    Data = new[] { "User Not Found!" }
                };
            }

            string hashedOldPassword = Infrasturcture.Extensions.EncryptPasswordExtension.EncryptPassword(request.OldPassword, user.Salt);

            if (user.Password != hashedOldPassword)
            {
                return new ApplicationResult
                {
                    Success = false,
                    Data = new[] { "Old password is incorrect!" }
                };
            }

            var newSalt = Infrasturcture.Extensions.EncryptPasswordExtension.Salt();
            string hashedNewPassword = Infrasturcture.Extensions.EncryptPasswordExtension.EncryptPassword(request.NewPassword, newSalt);

            user.Password = hashedNewPassword;
            user.Salt = newSalt;

            await _userRepository.CommitChanges();

            return new ApplicationResult
            {
                Success = true,
            };
        }
    }
}

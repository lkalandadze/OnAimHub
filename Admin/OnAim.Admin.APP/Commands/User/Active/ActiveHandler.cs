using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Exceptions;
using OnAim.Admin.APP.Helpers;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.Active
{
    public class ActiveHandler : ICommandHandler<ActiveCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _repository;
        private readonly IEmailService _emailService;

        public ActiveHandler(IRepository<Infrasturcture.Entities.User> repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public async Task<ApplicationResult> Handle(ActiveCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.Query().FirstOrDefaultAsync();

            if (user == null)
            {
                throw new BadRequestException("Invalid or expired activation token.");
            }

            var temporaryPassword = PasswordHelper.GenerateTemporaryPassword();
            var salt = Infrasturcture.Extensions.EncryptPasswordExtension.Salt();
            var hashedPassword = Infrasturcture.Extensions.EncryptPasswordExtension.EncryptPassword(temporaryPassword, salt);

            user.IsActive = true;
            user.Password = hashedPassword;
            user.Salt = salt;
            //user.ActivationToken = null;
            //user.ActivationTokenExpiration = null;

            await _repository.Store(user);
            await _repository.CommitChanges();

            await _emailService.SendActivationEmailAsync(
                  user.Email,
                  "Your Account is Activated",
                  temporaryPassword,
                  user.FirstName
              );

            return new ApplicationResult { Success = true, Data = "Your account has been activated successfully." };

        }
    }
}

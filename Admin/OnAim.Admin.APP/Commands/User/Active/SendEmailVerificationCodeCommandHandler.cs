using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Email;
using OnAim.Admin.APP.Exceptions;
using OnAim.Admin.APP.Extensions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.Globalization;
using System.Security.Cryptography;

namespace OnAim.Admin.APP.Commands.User.Active
{
    public class SendEmailVerificationCodeCommandHandler(
     IRepository<Infrasturcture.Entities.User> userManager,
     IEmailSender emailSender,
     ILogger<SendEmailVerificationCodeCommandHandler> logger
     ) : ICommandHandler<SendEmailVerificationCode, ApplicationResult>
    {
        public async Task<ApplicationResult> Handle(SendEmailVerificationCode command, CancellationToken cancellationToken)
        {
            command.NotBeNull();
            var identityUser = await userManager.Query(x => x.Email == command.Email).FirstOrDefaultAsync();

            identityUser.NotBeNull(new NotFoundException(command.Email));

            int randomNumber = RandomNumberGenerator.GetInt32(0, 1000000);
            string verificationCode = randomNumber.ToString("D6", CultureInfo.InvariantCulture);

            (string Email, string VerificationCode) model = (command.Email, verificationCode);

            string content =
                $"Welcome to shop application! Please verify your email with using this Code: {model.VerificationCode}.";

            string subject = "Verification Email";

            EmailObject emailObject = new EmailObject(command.Email, subject, content);

            await emailSender.SendAsync(emailObject);

            logger.LogInformation("Verification email sent successfully for userId:{UserId}", identityUser.Id);

            return new ApplicationResult { Success = true };
        }
    }
}

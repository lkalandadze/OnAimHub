using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Exceptions;
using OnAim.Admin.Shared.Helpers;

namespace OnAim.Admin.APP.Commands.User.ForgotPassword
{
    public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _userRepository;
        private readonly IEmailService _emailService;
        private readonly IValidator<ForgotPasswordCommand> _validator;
        private readonly ILogger<ForgotPasswordCommand> _logger;

        public ForgotPasswordCommandHandler(
            IRepository<Infrasturcture.Entities.User> userRepository,
            IEmailService emailService,
            IValidator<ForgotPasswordCommand> validator,
            ILogger<ForgotPasswordCommand> logger
            )
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _validator = validator;
            _logger = logger;
        }
        public async Task<ApplicationResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateUserCommand. Errors: {Errors}", validationResult.Errors);

                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.Query(x => x.Email == request.Email).FirstOrDefaultAsync();

            if ( user == null )
            {
                throw new NotFoundException("User Not Found");
            }

            var resetCode = ActivationCodeHelper.ActivationCode();
            var resetCodeExpiration = DateTime.UtcNow.AddHours(1);

            user.ResetCode = resetCode;
            user.ResetCodeExpiration = resetCodeExpiration;

            await _userRepository.CommitChanges();

            var resetLink = $"reset-password?token={resetCode}";

            //var emailBody = "" +
            //    "<!DOCTYPE html>\n<html>\n<head>\n    " +
            //    "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    " +
            //    "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n   " +
            //    " <title>Reset Password</title>\n</head>\n<body style=\"font-family: Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 20px;\">\n    " +
            //    "<h1>Account Verification</h1>\n    <p>Hi {firstName},</p>\n    <p>Thank you for registering. Please click the link below to verify your email address:</p>\n   " +
            //    " <p style=\"color: rgba(0, 167, 111, 1); font-size: 32px; font-weight: 800; line-height: 43.58px; margin: 16px 0 0;\">{code}</p>\n    <p><a href=\"{link}\">Verify your account</a></p>\n   " +
            //    " <p>If you didn't request this, please ignore this email.</p>\n    <p>Best regards,<br>Your Company</p>\n</body>\n</html>\n";


            string templatePath = Path.Combine("Templates", "Emails", "ForgotPassword.html");
            string htmlBody = await ReadEmailTemplateAsync(templatePath);

            htmlBody = htmlBody.Replace("{firstName}", user.FirstName)
                   .Replace("{code}", resetCode.ToString())
                   .Replace("{link}", resetLink);

            await _emailService.SendActivationEmailAsync(user.Email, "Password Reset", htmlBody);

            return new ApplicationResult { Success = true };

        }
        private async Task<string> ReadEmailTemplateAsync(string path)
        {
            return await File.ReadAllTextAsync(path);
        }
    }
}

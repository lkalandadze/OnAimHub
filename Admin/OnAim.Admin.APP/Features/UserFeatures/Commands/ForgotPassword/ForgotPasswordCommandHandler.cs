using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Shared.Helpers;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : BaseCommandHandler<ForgotPasswordCommand, ApplicationResult>
{
    private readonly IRepository<Domain.Entities.User> _userRepository;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(
        CommandContext<ForgotPasswordCommand> context,
        IRepository<Domain.Entities.User> userRepository,
        IEmailService emailService
        ) : base( context )
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    protected override async Task<ApplicationResult> ExecuteAsync(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var user = await _userRepository.Query(x => x.Email == request.Email).FirstOrDefaultAsync();

        if (user == null)
            throw new NotFoundException("User Not Found");          

        var resetCode = ActivationCodeHelper.ActivationCode();
        var resetCodeExpiration = DateTime.UtcNow.AddHours(1);

        user.ResetCode = resetCode;
        user.ResetCodeExpiration = resetCodeExpiration;

        await _userRepository.CommitChanges();

        var resetLink = $"reset-password?token={resetCode}";

        var htmlBody = "" +
            "<!DOCTYPE html>\n<html>\n<head>\n    " +
            "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    " +
            "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n   " +
            " <title>Reset Password</title>\n</head>\n<body style=\"font-family: Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 20px;\">\n    " +
            "<h1>Account Verification</h1>\n    <p>Hi {firstName},</p>\n    <p>Thank you for registering. Please click the link below to verify your email address:</p>\n   " +
            " <p style=\"color: rgba(0, 167, 111, 1); font-size: 32px; font-weight: 800; line-height: 43.58px; margin: 16px 0 0;\">{code}</p>\n    <p><a href=\"{link}\">Verify your account</a></p>\n   " +
            " <p>If you didn't request this, please ignore this email.</p>\n    <p>Best regards,<br>Your Company</p>\n</body>\n</html>\n";


        //string templatePath = Path.Combine("Templates", "Emails", "ForgotPassword.html");
        //string htmlBody = await ReadEmailTemplateAsync(templatePath);

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

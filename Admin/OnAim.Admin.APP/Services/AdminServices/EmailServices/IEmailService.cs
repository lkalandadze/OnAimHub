namespace OnAim.Admin.APP.Services.AdminServices.EmailServices;

public interface IEmailService
{
    Task SendActivationEmailAsync(string recipientEmail, string subject, string htmlBody);
}

namespace OnAim.Admin.APP.Services.Abstract
{
    public interface IEmailService
    {
        Task SendActivationEmailAsync(string recipientEmail, string subject, string temporaryPassword, string firstName);
    }
}

using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.Models;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

namespace OnAim.Admin.APP.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<SmtpSettings> _smtpSettings;

        private readonly string _templatePath;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings;

            _templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ActivationEmailTemplate.html");
        }
        public async Task SendActivationEmailAsync(string recipientEmail, string subject, string temporaryPassword, string firstName)
        {
            var fromAddress = new MailAddress("gokropiridze@onaim.io", "Gvantsa");
            var toAddress = new MailAddress(recipientEmail, firstName);

            string htmlBody = File.ReadAllText(_templatePath);

            htmlBody = htmlBody.Replace("{firstName}", firstName)
                               .Replace("{temporaryPassword}", temporaryPassword);

            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "log.png");
            var logoImage = new Attachment(logoPath)
            {
                ContentId = "logoImage"
            };

            var smtpClient = new SmtpClient(_smtpSettings.Value.Server)
            {
                Port = _smtpSettings.Value.Port,
                Credentials = new NetworkCredential(_smtpSettings.Value.Username, _smtpSettings.Value.Password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            GetEmbeddedImage(logoPath);

            await smtpClient.SendMailAsync(mailMessage);
        }

        private AlternateView GetEmbeddedImage(String filePath)
        {
            LinkedResource res = new LinkedResource(filePath);
            res.ContentId = Guid.NewGuid().ToString();
            string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }
    }
}

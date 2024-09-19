using System.Net.Mail;
using OnAim.Admin.APP.Services.Abstract;
using Microsoft.Extensions.Options;
using OnAim.Admin.Shared.Models;
using System.Net.Mime;
using Newtonsoft.Json.Linq;
using System.Text;
using OnAim.Admin.Shared.HtmlGenerators;

public class EmailService : IEmailService
{
    private readonly IOptions<SmtpSettings> _smtpSettings;
    private readonly IHtmlGenerator _htmlGenerator;
    private readonly HttpClient _httpClient;
    private readonly string _templateName;

    public EmailService(IOptions<SmtpSettings> smtpSettings, IHtmlGenerator htmlGenerator)
    {
        _smtpSettings = smtpSettings;
        _htmlGenerator = htmlGenerator;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_smtpSettings.Value.Username}:{_smtpSettings.Value.Password}"))}");
        _templateName = "ActivationEmailTemplate";
    }
    public async Task SendActivationEmailAsync(string recipientEmail, string subject, string htmlBody)
    {
        //var model = new
        //{
        //    FirstName = firstName,
        //    TemporaryPassword = temporaryPassword
        //};

        //string htmlBody = await _htmlGenerator.GenerateAsync(_templateName, model);

        //string htmlBody = "<!DOCTYPE html>\n<html>\n<head>\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n    <title>Email</title>\n    <style>\n        @media (prefers-color-scheme: dark) {\n            .background {\n                background-color: #EDEFF2 !important;\n            }\n\n            .container {\n                background-color: #ffffff !important;\n            }\n\n            .password-box {\n                background-color: rgba(0, 167, 111, 0.08) !important;\n            }\n        }\n    </style>\n</head>\n<body style=\"background-color: #EDEFF2; margin: 0; padding: 0; color: #000000; font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;\">\n    <div class=\"background\" style=\"max-width: 558px; width: 100%; height: 80px; margin: 48px auto;\">\n        <img src=\"cid:logoImage\" alt=\"logo\" />\n    </div>\n\n    <div class=\"container\" style=\"max-width: 558px; width: 100%; padding: 48px; background-color: white; margin: auto;\">\n        <p style=\"font-size: 16px; font-weight: 600; line-height: 21.79px; color: rgba(28, 37, 46, 1);\">Dear, {firstName}</p>\n        <p style=\"font-size: 16px; font-weight: 600; line-height: 21.79px; color: rgba(28, 37, 46, 1);\">Your Account Has Been Successfully Activated!</p>\n        <p style=\"font-size: 14px; font-weight: 400; line-height: 20px; color: rgba(28, 37, 46, 1);\">Here is the temporary password you need to access your account!</p>\n        <p style=\"font-size: 14px; font-weight: 400; line-height: 20px; color: rgba(28, 37, 46, 1);\">For safety reasons, we recommend changing your password.</p>\n\n        <div class=\"password-box\" style=\"max-width: 366px; width: 100%; height: 124px; background-color: rgba(0, 167, 111, 0.08); margin: 48px auto 240px; padding: 24px 111px; text-align: center;\">\n            <p style=\"color: #637381; font-size: 12px; font-weight: 600; line-height: 16.34px; margin: 0;\">Temporary password</p>\n            <p style=\"color: rgba(0, 167, 111, 1); font-size: 32px; font-weight: 800; line-height: 43.58px; margin: 16px 0 0;\">{temporaryPassword}</p>\n        </div>\n    </div>\n</body>\n</html>\n";

        //htmlBody = htmlBody.Replace("{firstName}", firstName)
        //                   .Replace("{temporaryPassword}", temporaryPassword);

        //string htmlBody = "<!DOCTYPE html>\n<html>\n<head>\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n    <title>Verify Your Account</title>\n</head>\n<body style=\"font-family: Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 20px;\">\n    <h1>Account Verification</h1>\n    <p>Hi  {firstName},</p>\n    <p>Thank you for registering. Please click the link below to verify your email address:</p>\n    <p style=\"color: rgba(0, 167, 111, 1); font-size: 32px; font-weight: 800; line-height: 43.58px; margin: 16px 0 0;\">{code}</p>\n    <p><a href=link>Verify your account</a></p>\n    <p>If you didn't request this, please ignore this email.</p>\n    <p>Best regards,<br>Your Company</p>\n</body>\n</html>\n";

        //htmlBody = htmlBody.Replace("{firstName}", firstName)
        //                   .Replace("{code}", temporaryPassword)
        //                   .Replace("{link}", link);

        var requestBody = new JObject
        {
            ["Messages"] = new JArray
        {
            new JObject
            {
                ["From"] = new JObject
                {
                    ["Email"] = "gokropiridze@onaim.io",
                    ["Name"] = "Gvantsa"
                },
                ["To"] = new JArray
                {
                    new JObject
                    {
                        ["Email"] = recipientEmail,
                        ["Name"] = "Recipient"
                    }
                },
                ["Subject"] = subject,
                ["HtmlPart"] = htmlBody
            }
        }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.mailjet.com/v3.1/send")
        {
            Content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error sending email: {responseContent}");
        }

        //var fromAddress = new MailAddress("gokropiridze@onaim.io", "Gvantsa");
        //var toAddress = new MailAddress(recipientEmail, firstName);

        //string htmlBody = File.ReadAllText(_templatePath);

        //htmlBody = htmlBody.Replace("{firstName}", firstName)
        //                   .Replace("{temporaryPassword}", temporaryPassword);

        //var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "log.png");
        //var logoImage = new Attachment(logoPath)
        //{
        //    ContentId = "logoImage"
        //};

        //var smtpClient = new SmtpClient(_smtpSettings.Value.Server)
        //{
        //    Port = _smtpSettings.Value.Port,
        //    Credentials = new NetworkCredential(_smtpSettings.Value.Username, _smtpSettings.Value.Password),
        //    EnableSsl = true,
        //};

        //var mailMessage = new MailMessage(fromAddress, toAddress)
        //{
        //    Subject = subject,
        //    Body = htmlBody,
        //    IsBodyHtml = true
        //};

        //GetEmbeddedImage(logoPath);

        //await smtpClient.SendMailAsync(mailMessage);
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

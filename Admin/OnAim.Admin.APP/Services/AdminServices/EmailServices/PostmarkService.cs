using OnAim.Admin.APP.Services.AdminServices.EmailServices;
using PostmarkDotNet;

namespace OnAim.Admin.APP.Services.Admin.EmailServices;

public class PostmarkService : IEmailService
{
    private readonly string _apiKey;

    public PostmarkService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task SendActivationEmailAsync(string recipientEmail, string subject, string htmlBody)
    {
        var client = new PostmarkClient(_apiKey);

        var message = new PostmarkMessage
        {
            From = "gokropiridze@onaim.io",
            To = recipientEmail,
            Subject = subject,
            HtmlBody = htmlBody
        };

        var result = await client.SendMessageAsync(message);

        if (result.Status != PostmarkStatus.Success)
        {
            Console.WriteLine($"Error: {result.Message}");
        }
        else
        {
            Console.WriteLine("Email sent successfully!");
        }
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var client = new PostmarkClient(_apiKey);

        var message = new PostmarkMessage
        {
            From = "gokropiridze@onaim.io",
            To = toEmail,
            Subject = subject,
            TextBody = body
        };

        var result = await client.SendMessageAsync(message);

        if (result.Status != PostmarkStatus.Success)
        {
            Console.WriteLine($"Error: {result.Message}");
        }
        else
        {
            Console.WriteLine("Email sent successfully!");
        }
    }
}

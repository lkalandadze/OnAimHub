using System.Net.Mail;
using OnAim.Admin.APP.Services.Abstract;
using Microsoft.Extensions.Options;
using OnAim.Admin.Shared.Models;
using System.Net.Mime;
using Newtonsoft.Json.Linq;
using System.Text;
using OnAim.Admin.Shared.Helpers.HtmlGenerators;

namespace OnAim.Admin.APP.Services.Email;

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

        var requestBody = new JObject
        {
            ["Messages"] = new JArray
        {
            new JObject
            {
                ["From"] = new JObject
                {
                    ["Email"] = "gokropiridze@onaim.io",
                    ["Name"] = "OnAim"
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

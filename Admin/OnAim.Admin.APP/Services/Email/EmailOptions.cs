using OnAim.Admin.APP.Services.Email.SendGrid;
using OnAim.Admin.APP.Services.Email.SmtpClient;

namespace OnAim.Admin.APP.Services.Email;

public class EmailOptions
{
    public string Provider { get; set; }

    public SmtpClientOptions SmtpClient { get; set; }

    public SendGridOptions SendGrid { get; set; }

    public bool UsedSmtpClient()
    {
        return Provider == "SmtpClient";
    }

    public bool UsedSendGrid()
    {
        return Provider == "SendGrid";
    }
}

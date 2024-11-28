using OnAim.Admin.APP.Services.Admin.EmailServices.SendGrid;
using OnAim.Admin.APP.Services.Admin.EmailServices.SmtpClient;

namespace OnAim.Admin.APP.Services.Admin.EmailServices;

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

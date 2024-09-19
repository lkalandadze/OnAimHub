using OnAim.Admin.APP.Email.SendGrid;
using OnAim.Admin.APP.Email.SmtpClient;

namespace OnAim.Admin.APP.Email
{
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

}

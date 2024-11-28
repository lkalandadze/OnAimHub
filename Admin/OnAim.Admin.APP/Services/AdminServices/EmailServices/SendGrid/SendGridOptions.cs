namespace OnAim.Admin.APP.Services.Admin.EmailServices.SendGrid;

public class SendGridOptions
{
    public string ApiKey { get; set; }

    public string OverrideFrom { get; set; }

    public string OverrideTos { get; set; }
}

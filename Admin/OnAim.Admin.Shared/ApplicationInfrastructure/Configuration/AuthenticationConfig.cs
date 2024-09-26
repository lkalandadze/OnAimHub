namespace OnAim.Admin.Shared.ApplicationInfrastructure.Configuration;

public class AuthenticationConfig
{
    public string Authority { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }

    public static new string ToString()
    {
        return "authentication";
    }
}

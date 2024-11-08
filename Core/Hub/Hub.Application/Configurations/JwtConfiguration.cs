namespace Hub.Application.Configurations;

public class JwtConfiguration
{
    public string PrivateKey { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public int ExpiresInMinutes { get; set; }
}
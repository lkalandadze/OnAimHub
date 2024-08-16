#nullable disable

namespace Hub.Application.Configurations;

public class JwtConfig
{
    public string ValidIssuer { get; set; }

    public string ValidAudience { get; set; }

    public List<string> ValidAudiences { get; set; }

    public string SecretKey { get; set; }

    public int ExpiresInMinutes { get; set; }
}
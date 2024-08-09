#nullable disable

namespace Hub.Application.Configurations;

public class JwtTokenConfiguration
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
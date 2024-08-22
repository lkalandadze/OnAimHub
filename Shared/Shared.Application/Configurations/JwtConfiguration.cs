#nullable disable

namespace Shared.Application.Configurations;

public class JwtConfiguration
{
    public string PublicKey { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }
}
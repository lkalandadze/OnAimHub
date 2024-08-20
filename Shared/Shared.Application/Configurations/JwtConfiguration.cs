namespace Shared.Application.Configurations;

public class JwtConfiguration
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string PublicKeyPath { get; set; }
}
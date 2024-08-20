namespace Hub.Application.Configurations;

public class JwtConfiguration
{
    public string PrivateKeyPath { get; set; }
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OnAim.Admin.Shared.ApplicationInfrastructure.Configuration;

public class JwtConfiguration
{
    public string Issuer { get; set; }
    public string Subject { get; set; }
    public string Secret { get; set; } = null!;
    public string Audience { get; set; }
    public TimeSpan ValidFor { get; set; } = TimeSpan.FromDays(1);
    public Func<string> JtiGenerator =>
      () => Guid.NewGuid().ToString();
    public SigningCredentials SigningCredentials
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Secret))
            {
                throw new ArgumentNullException(nameof(Secret), "Secret cannot be null or empty.");
            }

            return new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)),
                SecurityAlgorithms.HmacSha256);
        }
    }
}

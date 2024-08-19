using System.Security.Claims;

namespace OnAim.Admin.Infrasturcture.Configuration
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken(int userId, string email, IEnumerable<Claim> additionalClaims, IEnumerable<string> roles);
    }
}

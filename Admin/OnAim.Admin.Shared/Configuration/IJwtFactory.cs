using System.Security.Claims;

namespace OnAim.Admin.Shared.Configuration
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken(int userId, string email, IEnumerable<Claim> additionalClaims, IEnumerable<string> roles);
        Task SaveAccessToken(int userId, string token, DateTime expiration);
        Task<bool> RevokeAccessToken(string token);
        Task<string> GenerateRefreshToken(int userId);
        Task<bool> RevokeRefreshToken(string token);
        ClaimsPrincipal? GetPrincipalFromToken(string token);
    }
}

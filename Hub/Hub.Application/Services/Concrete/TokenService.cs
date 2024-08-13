using Hub.Application.Configurations;
using Hub.Application.Helpers;
using Hub.Application.Services.Abstract;
using Hub.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hub.Application.Services.Concrete;

public class TokenService(AppSettings appSettings) : ITokenService
{
    private readonly AppSettings _appSettings = appSettings;

    public SigningCredentials GetSigningCredentials()
    {
        var jwtConfig = _appSettings.JwtConfig;
        var key = Encoding.UTF8.GetBytes(jwtConfig.SecretKey);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    public List<Claim> GetClaims(Player player)
    {
        var claims = new List<Claim>
        {
            new("PlayerId", player.Id.ToString()),
            new("UserName", player.UserName),
        };

        return claims;
    }

    public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _appSettings.JwtConfig;

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings.ValidIssuer,
            audience: jwtSettings.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings.ExpiresInMinutes)),
            signingCredentials: signingCredentials);

        return tokenOptions;
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string expiredToken)
    {
        var tokenValidationParameters = TokenHelper.GetTokenValidationParametersForExpiredToken(_appSettings);

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(expiredToken, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
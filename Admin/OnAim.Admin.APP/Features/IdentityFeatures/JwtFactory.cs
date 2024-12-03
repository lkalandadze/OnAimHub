using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;
using OnAim.Admin.Contracts.ApplicationInfrastructure.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OnAim.Admin.APP.Extensions;

namespace OnAim.Admin.APP.Feature.Identity;

public class JwtFactory : IJwtFactory
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly DatabaseContext _context;

    public JwtFactory(IOptions<JwtConfiguration> jwtOptions, DatabaseContext context)
    {
        _jwtConfiguration = jwtOptions.Value;
        _context = context;
        ThrowIfInvalidOptions(_jwtConfiguration);
    }

    public string GenerateEncodedToken(int userId, string email, IEnumerable<Claim> additionalClaims, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
    {
        new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, userId.ToString()),
        new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email, email),
        new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, _jwtConfiguration.JtiGenerator()),
        new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64),
    };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        claims.AddRange(additionalClaims);

        var jwt = new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.Add(_jwtConfiguration.ValidFor),
            signingCredentials: _jwtConfiguration.SigningCredentials);

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }

    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        token.NotBeNullOrWhiteSpace();
        _jwtConfiguration.Secret.NotBeNullOrWhiteSpace();

        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret)),
            ValidateLifetime = true,
            ClockSkew = System.TimeSpan.Zero,
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        ClaimsPrincipal principal = tokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out SecurityToken securityToken
        );

        JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null)
            throw new SecurityTokenException("Invalid access token.");

        return principal;
    }

    public async Task SaveAccessToken(int userId, string token, DateTime expiration)
    {
        var accessToken = new AccessToken(userId, token, expiration);

        _context.AccessTokens.Add(accessToken);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> RevokeAccessToken(string token)
    {
        var accessToken = await _context.AccessTokens
            .FirstOrDefaultAsync(at => at.Token == token);

        if (accessToken == null)
            return false;

        _context.AccessTokens.Remove(accessToken);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<string> GenerateRefreshToken(int userId)
    {
        var refreshToken = new RefreshToken(userId, Guid.NewGuid().ToString(), DateTime.UtcNow.AddMonths(1), false);

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken.Token;
    }

    public async Task<bool> RevokeRefreshToken(string token)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);

        if (refreshToken == null || refreshToken.IsRevoked)
            return false;

        refreshToken.IsRevoked = true;
        await _context.SaveChangesAsync();

        return true;
    }

    private static long ToUnixEpochDate(DateTime date)
      => (long)Math.Round((date.ToUniversalTime() -
                           new DateTimeOffset(1970, 1, 1, 0, 0, 0, System.TimeSpan.Zero))
                          .TotalSeconds);

    private static void ThrowIfInvalidOptions(JwtConfiguration options)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        if (options.ValidFor <= System.TimeSpan.Zero)
            throw new ArgumentException("Must be a non-zero TimeSpan.");

        if (options.SigningCredentials == null)
            throw new ArgumentException(nameof(JwtConfiguration.SigningCredentials));

        if (options.JtiGenerator == null)
            throw new ArgumentException(nameof(JwtConfiguration.JtiGenerator));
    }
}

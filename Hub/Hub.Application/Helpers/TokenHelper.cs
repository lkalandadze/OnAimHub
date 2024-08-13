using Hub.Application.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Hub.Application.Helpers;

public static class TokenHelper
{
    public static TokenValidationParameters GetTokenValidationParameters(AppSettings appSettings)
    {
        var jwtOptions = appSettings.JwtConfig;
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.ValidIssuer,
            ValidAudiences = jwtOptions.ValidAudiences,
            ValidAudience = jwtOptions.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
            ClockSkew = TimeSpan.Zero,
        };

        return tokenValidationParameters;
    }

    public static TokenValidationParameters GetTokenValidationParametersForExpiredToken(AppSettings appSettings)
    {
        var jwtOptions = appSettings.JwtConfig;
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidIssuer = jwtOptions.ValidIssuer,
            ValidAudiences = jwtOptions.ValidAudiences,
            ValidAudience = jwtOptions.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
        };

        return tokenValidationParameters;
    }
}
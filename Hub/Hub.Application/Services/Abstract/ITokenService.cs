using Hub.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Hub.Application.Services.Abstract;

public interface ITokenService
{
    SigningCredentials GetSigningCredentials();

    List<Claim> GetClaims(Player player);

    JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string expiredToken);
}
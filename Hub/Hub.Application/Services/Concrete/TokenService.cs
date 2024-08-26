using Hub.Application.Configurations;
using Hub.Application.Services.Abstract;
using Hub.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Hub.Application.Services.Concrete;

public class TokenService : ITokenService
{
    private readonly JwtConfiguration _jwtConfig;

    public TokenService(IOptions<JwtConfiguration> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
    }

    public string GenerateTokenString(Player player)
    {
        var claims = new List<Claim>
        {
            new("PlayerId", player.Id.ToString()),
            new("UserName", player.UserName),
            new Claim("SegmentIds", string.Join(",", player.SegmentIds)),
        };

        var ecdsaSecurityKey = GetECDsaKeyFromPrivateKey(_jwtConfig.PrivateKey);
        var signingCred = new SigningCredentials(ecdsaSecurityKey, SecurityAlgorithms.EcdsaSha256);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            audience: _jwtConfig.Audience,
            issuer: _jwtConfig.Issuer,
            expires: DateTime.Now.AddMinutes(20),
            signingCredentials: signingCred
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    private ECDsaSecurityKey GetECDsaKeyFromPrivateKey(string privateKey)
    {
        var ecdsaKey = ECDsa.Create();
        var keyBytes = Convert.FromBase64String(privateKey);
        ecdsaKey.ImportECPrivateKey(keyBytes, out _);

        return new ECDsaSecurityKey(ecdsaKey);
    }
}
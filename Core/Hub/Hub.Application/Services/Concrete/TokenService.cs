﻿using Hub.Application.Configurations;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Hub.Application.Services.Concrete;

public class TokenService : ITokenService
{
    private readonly JwtConfiguration _jwtConfig;
    private readonly ITokenRecordRepository _tokenRecordRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TokenService(IOptions<JwtConfiguration> jwtConfig, ITokenRecordRepository tokenRecordRepository, IPlayerRepository playerRepository, IUnitOfWork unitOfWork)
    {
        _jwtConfig = jwtConfig.Value;
        _tokenRecordRepository = tokenRecordRepository;
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<(string AccessToken, string RefreshToken)> GenerateTokenStringAsync(Player player)
    {
        var existingTokens = _tokenRecordRepository.Query()
            .Where(tr => tr.PlayerId == player.Id && !tr.IsRevoked && tr.RefreshTokenExpiryDate > DateTime.UtcNow);

        foreach (var token in existingTokens)
        {
            token.SetRevoked();
        }

        await _unitOfWork.SaveAsync();

        var claims = new List<Claim>
        {
            new("PlayerId", player.Id.ToString()),
            new("UserName", player.UserName),
            new Claim("SegmentIds", string.Join(",", player.Segments.Select(x => x.Id))),
        };

        var ecdsaSecurityKey = GetECDsaKeyFromPrivateKey(_jwtConfig.PrivateKey);
        var signingCred = new SigningCredentials(ecdsaSecurityKey, SecurityAlgorithms.EcdsaSha256);

        var accessToken = new JwtSecurityToken(
            claims: claims,
            audience: _jwtConfig.Audience,
            issuer: _jwtConfig.Issuer,
            expires: DateTime.Now.AddMinutes(_jwtConfig.ExpiresInMinutes),
            signingCredentials: signingCred
        );

        var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        var tokenRecord = new TokenRecord(accessTokenString, refreshToken, player.Id, accessToken.ValidTo, DateTime.UtcNow.AddDays(1));

        await _tokenRecordRepository.InsertAsync(tokenRecord);
        await _unitOfWork.SaveAsync();

        return (accessTokenString, refreshToken);
    }

    public async Task<(string AccessToken, string RefreshToken)> RefreshAccessTokenAsync(string accessToken, string refreshToken)
    {
        var tokenRecord = _tokenRecordRepository.Query()
            .FirstOrDefault(tr => tr.RefreshToken == refreshToken);

        if (tokenRecord == null || tokenRecord.IsRevoked || tokenRecord.RefreshTokenExpiryDate <= DateTime.UtcNow)
        {
            throw new ApiException(
                ApiExceptionCodeTypes.BusinessRuleViolation,
                "Invalid or expired refresh token: The token is either missing, revoked, or has expired."
            );
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(accessToken);

        int playerIdFromToken = int.Parse(jwtToken.Claims.First(c => c.Type == "PlayerId").Value);

        if (jwtToken == null || !jwtToken.Claims.Any() || tokenRecord.PlayerId != playerIdFromToken)
        {
            throw new ApiException(
                ApiExceptionCodeTypes.BusinessRuleViolation,
                $"Invalid access token: The token is either missing, contains no claims, or does not match the provided player ID."
            );
        }

        tokenRecord.SetRevoked();

        await _unitOfWork.SaveAsync();

        var player = await _playerRepository.OfIdAsync(tokenRecord.PlayerId);

        if (player == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Player with the specified ID: [{tokenRecord.PlayerId}] was not found.");
        }

        (string newAccessToken, string newRefreshToken) = await GenerateTokenStringAsync(player);

        return (newAccessToken, newRefreshToken);
    }

    private ECDsaSecurityKey GetECDsaKeyFromPrivateKey(string privateKey)
    {
        var ecdsaKey = ECDsa.Create();
        var keyBytes = Convert.FromBase64String(privateKey);
        ecdsaKey.ImportECPrivateKey(keyBytes, out _);

        return new ECDsaSecurityKey(ecdsaKey);
    }
}
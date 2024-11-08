using Hub.Domain.Entities;

namespace Hub.Application.Services.Abstract;

public interface ITokenService
{
    Task<(string AccessToken, string RefreshToken)> GenerateTokenStringAsync(Player player);
    Task<(string AccessToken, string RefreshToken)> RefreshAccessTokenAsync(string accessToken, string refreshToken);
}
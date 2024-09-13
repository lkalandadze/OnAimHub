#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class TokenRecord : BaseEntity<int>
{
    public TokenRecord()
    {

    }

    public TokenRecord(string accessToken, string refreshToken, int playerId, DateTime accessTokenExpiryDate, DateTime refreshTokenExpiryDate)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        PlayerId = playerId;
        AccessTokenExpiryDate = accessTokenExpiryDate;
        RefreshTokenExpiryDate = refreshTokenExpiryDate;
        IsRevoked = false;
        CreatedDate = DateTime.UtcNow;
    }

    public string AccessToken { get; private set; }
    public string RefreshToken { get; private set; }
    public int PlayerId { get; private set; }
    public DateTime AccessTokenExpiryDate { get; private set; }
    public DateTime RefreshTokenExpiryDate { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime? RevokedDate { get; private set; }

    public void SetRevoked()
    {
        RevokedDate = DateTime.UtcNow;
        IsRevoked = true;
    }
}
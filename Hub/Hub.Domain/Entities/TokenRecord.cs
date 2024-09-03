using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class TokenRecord : BaseEntity<int>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int PlayerId { get; set; }
    public DateTime AccessTokenExpiryDate { get; set; }
    public DateTime RefreshTokenExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? RevokedDate { get; set; }
}

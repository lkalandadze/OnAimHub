using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public RefreshToken()
    {
        
    }
    public RefreshToken(int userId, string token, DateTime expiration, bool isRevoked)
    {
        UserId = userId;
        Token = token;
        Expiration = expiration;
        IsRevoked = isRevoked;
        DateCreated = SystemDate.Now;
        IsActive = true;
    }

    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public bool IsRevoked { get; set; }
    public User User { get; set; }
}

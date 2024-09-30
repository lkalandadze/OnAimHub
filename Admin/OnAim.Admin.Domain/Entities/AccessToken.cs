using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Domain.Entities;

public class AccessToken : BaseEntity
{
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public User User { get; set; }

    public AccessToken(int userId, string token, DateTime expiration)
    {
        UserId = userId;
        Token = token;
        Expiration = expiration;
        DateCreated = SystemDate.Now;
    }
}

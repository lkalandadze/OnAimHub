using OnAim.Admin.Domain.Entities.Abstract;

namespace OnAim.Admin.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public bool IsRevoked { get; set; }
    public User User { get; set; }
}

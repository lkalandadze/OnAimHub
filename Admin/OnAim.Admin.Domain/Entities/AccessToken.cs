using OnAim.Admin.Domain.Entities.Abstract;

namespace OnAim.Admin.Domain.Entities;

public class AccessToken : BaseEntity
{
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public User User { get; set; }
}

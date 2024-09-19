using OnAim.Admin.Infrasturcture.Entities.Abstract;

namespace OnAim.Admin.Infrasturcture.Entities
{
    public class AccessToken : BaseEntity
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public User User { get; set; }
    }
}

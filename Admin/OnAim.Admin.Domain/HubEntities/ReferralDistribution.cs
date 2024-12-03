using OnAim.Admin.Domain.HubEntities.PlayerEntities;

namespace OnAim.Admin.Domain.HubEntities;

// Generated Code

public class ReferralDistribution : BaseEntity<Int32>	{
    public int ReferrerId { get; set; }
    public int ReferralId { get; set; }
    public int ReferrerPrizeValue { get; set; }
    public string ReferrerPrizeId { get; set; }
    public Currency ReferrerPrizeCurrency { get; set; }
    public int ReferralPrizeValue { get; set; }
    public string ReferralPrizeId { get; set; }
    public Currency ReferralPrizeCurrency { get; set; }
    public Player Referrer { get; set; }
    public Player Referral { get; set; }
    public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;
}

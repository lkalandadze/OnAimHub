#nullable disable

using Hub.Domain.Entities.DbEnums;
using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

[GloballyVisible]
public class ReferralDistribution : BaseEntity<int>
{
    public ReferralDistribution() { }

    public ReferralDistribution(int referrerId, int referralId, string referrerPrizeId, int referrerPrizeValue, Currency referrerPrizeCurrency, int referralPrizeValue, string referralPrizeId, Currency referralPrizeCurrency)
    {
        ReferrerId = referrerId;
        ReferralId = referralId;
        ReferrerPrizeId = referrerPrizeId;
        ReferrerPrizeValue = referrerPrizeValue;
        ReferrerPrizeCurrency = referrerPrizeCurrency;
        ReferralPrizeValue = referralPrizeValue;
        ReferralPrizeId = referralPrizeId;
        ReferralPrizeCurrency = referralPrizeCurrency;
    }

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
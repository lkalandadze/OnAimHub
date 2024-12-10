#nullable disable

using Hub.Domain.Entities.Coins;
using Hub.Domain.Entities.DbEnums;
using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

[GloballyVisible]
public class ReferralDistribution : BaseEntity<int>
{
    public ReferralDistribution() { }

    public ReferralDistribution(int referrerId, int referralId, string referrerPrizeId, int referrerPrizeValue, Coin referrerPrizeCoin, int referralPrizeValue, string referralPrizeId, Coin referralPrizeCoin)
    {
        ReferrerId = referrerId;
        ReferralId = referralId;
        ReferrerPrizeId = referrerPrizeId;
        ReferrerPrizeValue = referrerPrizeValue;
        ReferrerPrizeCoin = referrerPrizeCoin;
        ReferralPrizeValue = referralPrizeValue;
        ReferralPrizeId = referralPrizeId;
        ReferralPrizeCoin = referralPrizeCoin;
    }

    public int ReferrerId { get; set; }
    public int ReferralId { get; set; }
    public int ReferrerPrizeValue { get; set; }
    public string ReferrerPrizeId { get; set; }
    public Coin ReferrerPrizeCoin { get; set; }
    public int ReferralPrizeValue { get; set; }
    public string ReferralPrizeId { get; set; }
    public Coin ReferralPrizeCoin { get; set; }
    public Player Referrer { get; set; }
    public Player Referral { get; set; }
    public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;
}
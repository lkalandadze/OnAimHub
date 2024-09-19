using Hub.Domain.Entities;

namespace Hub.Application.Features.ReferralFeatures.ReferralDistributionFeatures.Dtos;

public class ReferralDistributionDto
{
    public int Id { get; set; }
    public int ReferrerId { get; set; }
    public int ReferralId { get; set; }
    public string ReferralPrizeId { get; set; }
    public int ReferrerPrizeValue { get; set; }
    public string ReferrerPrizeId { get; set; }
    public int ReferralPrizeValue { get; set; }
    public static ReferralDistributionDto MapFrom(ReferralDistribution distribution)
    {
        return new ReferralDistributionDto
        {
            Id = distribution.Id,
            ReferrerId = distribution.ReferrerId,
            ReferralId = distribution.ReferralId,
            ReferralPrizeId = distribution.ReferralPrizeId,
            ReferralPrizeValue = distribution.ReferralPrizeValue,
            ReferrerPrizeId = distribution.ReferrerPrizeId,
            ReferrerPrizeValue = distribution.ReferrerPrizeValue,
        };
    }
}
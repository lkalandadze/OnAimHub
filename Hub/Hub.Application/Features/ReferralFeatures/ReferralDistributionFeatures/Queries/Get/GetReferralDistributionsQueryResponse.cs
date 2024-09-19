using Hub.Application.Features.ReferralFeatures.ReferralDistributionFeatures.Dtos;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.ReferralFeatures.ReferralDistributionFeatures.Queries.Get;


public class GetReferralDistributionsQueryResponse : Response<PagedResponse<ReferralDistributionDto>>;

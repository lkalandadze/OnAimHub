using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.ReferralFeatures.ReferralDistributionFeatures.Queries.Get;

public class GetReferralDistributionsQuery : PagedRequest, IRequest<GetReferralDistributionsQueryResponse>;
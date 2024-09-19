using MediatR;
using Shared.Lib.Wrappersl;

namespace Hub.Application.Features.ReferralFeatures.ReferralDistributionFeatures.Queries.Get;

public class GetReferralDistributionsQuery : PagedRequest, IRequest<GetReferralDistributionsQueryResponse>;
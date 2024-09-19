using MediatR;

namespace Hub.Application.Features.ReferralFeatures.ReferralDistributionFeatures.Queries.GetById;

public record GetReferralDistributionByIdQuery(int Id) : IRequest<GetReferralDistributionByIdQueryResponse>;
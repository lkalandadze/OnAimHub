using Hub.Application.Features.ReferralFeatures.ReferralDistributionFeatures.Dtos;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.ReferralFeatures.ReferralDistributionFeatures.Queries.Get;

public class GetReferralDistributionsQueryHandler : IRequestHandler<GetReferralDistributionsQuery, GetReferralDistributionsQueryResponse>
{
    private readonly IReferralDistributionRepository _referralDistributionRepository;
    public GetReferralDistributionsQueryHandler(IReferralDistributionRepository referralDistributionRepository)
    {
        _referralDistributionRepository = referralDistributionRepository;
    }

    public async Task<GetReferralDistributionsQueryResponse> Handle(GetReferralDistributionsQuery request, CancellationToken cancellationToken)
    {
        var prizes = _referralDistributionRepository.Query();

        var total = prizes.Count();

        var prizeList = prizes.Pagination(request).ToList();

        var response = new GetReferralDistributionsQueryResponse
        {
            Data = new PagedResponse<ReferralDistributionDto>
            (
                prizeList?.Select(x => ReferralDistributionDto.MapFrom(x)),
                request.PageNumber,
                request.PageSize,
                total
            ),
        };

        return response;
    }
}
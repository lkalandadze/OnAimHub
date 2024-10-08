﻿using Hub.Application.Features.ReferralFeatures.ReferralDistributionFeatures.Dtos;
using Hub.Domain.Absractions.Repository;
using MediatR;

namespace Hub.Application.Features.ReferralFeatures.ReferralDistributionFeatures.Queries.GetById;

public class GetReferralDistributionByIdQueryHandler : IRequestHandler<GetReferralDistributionByIdQuery, GetReferralDistributionByIdQueryResponse>
{
    private readonly IReferralDistributionRepository _referralDistributionRepository;
    public GetReferralDistributionByIdQueryHandler(IReferralDistributionRepository referralDistributionRepository)
    {
        _referralDistributionRepository = referralDistributionRepository;
    }

    public async Task<GetReferralDistributionByIdQueryResponse> Handle(GetReferralDistributionByIdQuery request, CancellationToken cancellationToken)
    {
        var distribution = _referralDistributionRepository.Query().FirstOrDefault(x => x.Id == request.Id);

        if (distribution == default)
            throw new Exception("Referral prize not found");

        var distributionDto = ReferralDistributionDto.MapFrom(distribution);

        var response = new GetReferralDistributionByIdQueryResponse
        {
            Data = distributionDto
        };

        return response;
    }
}
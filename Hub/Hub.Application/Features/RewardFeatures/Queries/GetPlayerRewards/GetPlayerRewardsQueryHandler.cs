﻿using Hub.Application.Features.RewardFeatures.Dtos;
using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hub.Application.Features.RewardFeatures.Queries.GetPlayerRewards;

public class GetPlayerRewardsQueryHandler : IRequestHandler<GetPlayerRewardsQuery, GetPlayerRewardsQueryResponse>
{
    private readonly IRewardRepository _rewardRepository;
    private readonly IAuthService _authService;

    public GetPlayerRewardsQueryHandler(IRewardRepository rewardRepository, IAuthService authService)
    {
        _rewardRepository = rewardRepository;
        _authService = authService;
    }

    public async Task<GetPlayerRewardsQueryResponse> Handle(GetPlayerRewardsQuery request, CancellationToken cancellationToken)
    {
        var playerId = _authService.GetCurrentPlayerId();

        var rewards = await _rewardRepository.Query(r => r.PlayerId == playerId && !r.IsDeleted)
                                             .Include(r => r.Source)
                                             .Include(r => r.Player)
                                             .Include(r => r.Prizes)
                                             .ToListAsync();

        return new GetPlayerRewardsQueryResponse
        {
            Succeeded = true,
            Data = rewards.Select(r => RewardDtoModel.MapFrom(r)),
        };
    }
}
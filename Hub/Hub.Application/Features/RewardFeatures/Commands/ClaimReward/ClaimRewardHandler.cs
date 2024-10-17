using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hub.Application.Features.RewardFeatures.Commands.ClaimReward;

public class ClaimRewardHandler : IRequestHandler<ClaimRewardCommand>
{
    private readonly IRewardRepository _rewardRepository;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public ClaimRewardHandler(IRewardRepository claimRepository, IAuthService authService, IUnitOfWork unitOfWork)
    {
        _rewardRepository = claimRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ClaimRewardCommand request, CancellationToken cancellationToken)
    {
        var playerId = _authService.GetCurrentPlayerId();

        var reward = _rewardRepository.Query(r => r.Id == request.RewardId && r.PlayerId == playerId && !r.IsDeleted)
                                      .Include(r => r.Prizes)
                                      .FirstOrDefault();

        if (reward == null)
        {
            throw new KeyNotFoundException($"Reward not fount for Id: {request.RewardId}");
        }

        if (reward.IsClaimed)
        {
            throw new KeyNotFoundException($"Reward is already claimed for Id: {request.RewardId}");
        }

        //TODO: deposit and transaction

        reward.SetAsClaimed();

        _rewardRepository.Update(reward);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}
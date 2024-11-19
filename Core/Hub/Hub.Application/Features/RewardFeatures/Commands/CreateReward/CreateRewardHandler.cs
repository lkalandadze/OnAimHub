using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using MediatR;

namespace Hub.Application.Features.PrizeClaimFeatures.Commands.CreateReward;

public class CreateRewardHandler : IRequestHandler<CreateRewardCommand>
{
    private readonly IRewardRepository _rewardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRewardHandler(IRewardRepository claimRepository, IUnitOfWork unitOfWork)
    {
        _rewardRepository = claimRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateRewardCommand request, CancellationToken cancellationToken)
    {
        var prizes = request.Prizes.Select(p => new RewardPrize(p.Amount, p.PrizeTypeId));

        var reward = new Reward(request.PlayerId, request.SourceId, request.ExpirationDate, prizes);

        await _rewardRepository.InsertAsync(reward);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}
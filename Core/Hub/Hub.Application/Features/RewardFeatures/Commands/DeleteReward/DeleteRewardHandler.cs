using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;

namespace Hub.Application.Features.PrizeClaimFeatures.Commands.DeleteReward;

public class DeleteRewardHandler : IRequestHandler<DeleteRewardCommand>
{
    private readonly IRewardRepository _rewardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRewardHandler(IRewardRepository claimRepository, IUnitOfWork unitOfWork)
    {
        _rewardRepository = claimRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteRewardCommand request, CancellationToken cancellationToken)
    {
        var reward = await _rewardRepository.OfIdAsync(request.Id);

        if (reward == null)
        {
            throw new KeyNotFoundException($"Reward not fount for Id: {request.Id}");
        }

        reward.Delete();

        _rewardRepository.Update(reward);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}
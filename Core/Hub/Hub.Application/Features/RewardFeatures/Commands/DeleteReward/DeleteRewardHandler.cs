using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

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
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Reward with the specified ID: [{request.Id}] was not found.");
        }

        reward.Delete();

        _rewardRepository.Update(reward);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}
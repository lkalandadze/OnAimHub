using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.RewardFeatures.Commands.ClaimReward;

public class ClaimRewardHandler : IRequestHandler<ClaimRewardCommand>
{
    private readonly IRewardRepository _rewardRepository;
    private readonly ITransactionService _transactionService;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public ClaimRewardHandler(IRewardRepository claimRepository, ITransactionService transactionService, IAuthService authService, IUnitOfWork unitOfWork)
    {
        _rewardRepository = claimRepository;
        _transactionService = transactionService;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ClaimRewardCommand request, CancellationToken cancellationToken)
    {
        var playerId = _authService.GetCurrentPlayerId();

        var reward = _rewardRepository.Query(r => r.Id == request.RewardId && r.PlayerId == playerId && !r.IsDeleted)
                                      .Include(r => r.Source)
                                      .Include(r => r.Prizes)
                                      .FirstOrDefault();

        if (reward == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Reward with the specified ID: [{request.RewardId}] was not found.");

        }

        if (reward.IsClaimed || reward.IsDeleted || reward.ExpirationDate < DateTime.UtcNow)
        {
            throw new ApiException(ApiExceptionCodeTypes.ValidationFailed, $"Reward with the specified ID: [{request.RewardId}] is unavailable.");
        }

        //TODO: prizes with actions (no currency)

        foreach (var prize in reward.Prizes)
        {
            if (prize.Coins != null)
            {
                var transactionType = DetermineTransactionType(reward.Source);

                await _transactionService.CreateTransactionAndApplyBalanceAsync(null, prize.CoinId, prize.Value, AccountType.Casino, AccountType.Player, transactionType, prize.Coins.PromotionId);
            }
        }

        reward.SetAsClaimed();

        _rewardRepository.Update(reward);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }

    private TransactionType DetermineTransactionType(RewardSource source)
    {
        if (source == RewardSource.Level)
        {
            return TransactionType.Level;
        }
        else if (source == RewardSource.Mission)
        {
            return TransactionType.Mission;
        }
        else if (source == RewardSource.Leaderboard)
        {
            return TransactionType.Leaderboard;
        }

        return TransactionType.Reward;
    }
}
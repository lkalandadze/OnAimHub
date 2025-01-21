using Hub.Application.Features.RewardFeatures.Commands.ReceiveReward;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities.DbEnums;
using Hub.Domain.Entities;
using MassTransit;
using MediatR;
using Shared.IntegrationEvents.IntegrationEvents.Reward.Leaderboard;
using Hub.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Hub.Domain.Enum;

namespace Hub.Application.Consumers.Rewards;

public sealed class ReceiveLeaderboardRewardAggregationConsumer : IConsumer<ReceiveLeaderboardRewardEvent>
{
    private readonly IPlayerBalanceRepository _playerBalanceRepository;
    private readonly ITransactionService _transactionService;
    private readonly ICoinRepository _coinRepository;
    private readonly IRewardRepository _rewardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReceiveLeaderboardRewardAggregationConsumer(
        IPlayerBalanceRepository playerBalanceRepository,
        ITransactionService transactionService,
        ICoinRepository coinRepository,
        IRewardRepository rewardRepository,
        IUnitOfWork unitOfWork)
    {
        _playerBalanceRepository = playerBalanceRepository;
        _transactionService = transactionService;
        _coinRepository = coinRepository;
        _rewardRepository = rewardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<ReceiveLeaderboardRewardEvent> context)
    {
        var data = context.Message;

        var groupedRewards = data.Rewards
            .GroupBy(r => new { r.PlayerId, r.CoinId, r.PromotionId, r.KeyId, r.SourceServiceName })
            .Select(g => new
            {
                g.Key.PlayerId,
                g.Key.CoinId,
                TotalAmount = g.Sum(r => r.Amount),
                g.Key.PromotionId,
                g.Key.KeyId,
                g.Key.SourceServiceName
            })
            .ToList();

        foreach (var reward in groupedRewards)
        {
            var coin = await _coinRepository.Query()
                .FirstOrDefaultAsync(x => x.Id == reward.CoinId, context.CancellationToken);

            if (coin.CoinType == CoinType.Asset)
            {
                var rewardPrizes = new List<RewardPrize>
                {
                    new RewardPrize((int)reward.TotalAmount, reward.CoinId)
                };

                var playerReward = new Reward(
                    isClaimableByPlayer: false,
                    playerId: reward.PlayerId,
                    sourceId: reward.PromotionId,
                    expirationDate: DateTime.UtcNow.AddDays(30),
                    prizes: rewardPrizes
                );

                await _rewardRepository.InsertAsync(playerReward);
            }
            else
            {
                var playerBalance = await _playerBalanceRepository.Query()
                    .FirstOrDefaultAsync(pb => pb.PlayerId == reward.PlayerId && pb.CoinId == reward.CoinId, context.CancellationToken);

                if (playerBalance != null)
                {
                    var newAmount = playerBalance.Amount + reward.TotalAmount;
                    playerBalance.SetAmount(newAmount);
                    _playerBalanceRepository.Update(playerBalance);
                }
                else
                {
                    playerBalance = new PlayerBalance(reward.TotalAmount, reward.PlayerId, reward.CoinId, reward.PromotionId);
                    await _playerBalanceRepository.InsertAsync(playerBalance);
                }

                await _transactionService.CreateLeaderboardTransactionAndApplyBalanceAsync(
                    reward.KeyId,
                    reward.SourceServiceName,
                    reward.CoinId,
                    reward.TotalAmount,
                    AccountType.Leaderboard,
                    AccountType.Player,
                    TransactionType.Reward,
                    reward.PromotionId,
                    reward.PlayerId);
            }
        }

        await _unitOfWork.SaveAsync();
    }
}
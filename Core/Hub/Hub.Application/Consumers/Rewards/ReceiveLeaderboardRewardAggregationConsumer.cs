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

namespace Hub.Application.Consumers.Rewards;

public sealed class ReceiveLeaderboardRewardAggregationConsumer : IConsumer<ReceiveLeaderboardRewardEvent>
{
    private readonly IPlayerBalanceRepository _playerBalanceRepository;
    private readonly ITransactionService _transactionService;
    private readonly IUnitOfWork _unitOfWork;

    public ReceiveLeaderboardRewardAggregationConsumer(
        IPlayerBalanceRepository playerBalanceRepository,
        ITransactionService transactionService,
        IUnitOfWork unitOfWork)
    {
        _playerBalanceRepository = playerBalanceRepository;
        _transactionService = transactionService;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<ReceiveLeaderboardRewardEvent> context)
    {
        var data = context.Message;

        var groupedRewards = data.Rewards
            .GroupBy(r => new { r.PlayerId, r.CoinId, r.PromotionId })
            .Select(g => new
            {
                g.Key.PlayerId,
                g.Key.CoinId,
                TotalAmount = g.Sum(r => r.Amount),
                g.Key.PromotionId
            })
            .ToList();

        foreach (var reward in groupedRewards)
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

            await _transactionService.CreateTransactionAndApplyBalanceAsync(
                reward.PlayerId,
                reward.CoinId,
                reward.TotalAmount,
                AccountType.Casino,
                AccountType.Player,
                TransactionType.Reward,
                null);
        }

        await _unitOfWork.SaveAsync();
    }
}
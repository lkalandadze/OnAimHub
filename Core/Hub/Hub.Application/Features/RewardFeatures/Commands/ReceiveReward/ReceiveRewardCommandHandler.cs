using Hub.Application.Features.RewardFeatures.Commands.ClaimReward;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities.DbEnums;
using MediatR;
using Shared.Application.Exceptions.Types;
using Shared.Application.Exceptions;
using Hub.Domain.Abstractions;
using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hub.Application.Features.RewardFeatures.Commands.ReceiveReward;

//public class ReceiveRewardCommandHandler : IRequestHandler<ReceiveRewardCommand>
//{
//    private readonly IPlayerBalanceRepository _playerBalanceRepository;
//    private readonly ITransactionService _transactionService;
//    private readonly IUnitOfWork _unitOfWork;

//    public ReceiveRewardCommandHandler(
//        IPlayerBalanceRepository playerBalanceRepository,
//        ITransactionService transactionService,
//        IUnitOfWork unitOfWork)
//    {
//        _playerBalanceRepository = playerBalanceRepository;
//        _transactionService = transactionService;
//        _unitOfWork = unitOfWork;
//    }

//    public async Task<Unit> Handle(ReceiveRewardCommand request, CancellationToken cancellationToken)
//    {
//        // Group rewards by PlayerId and CoinId for efficient batch processing
//        var groupedRewards = request.Rewards
//            .GroupBy(r => new { r.PlayerId, r.CoinId })
//            .Select(g => new
//            {
//                g.Key.PlayerId,
//                g.Key.CoinId,
//                TotalAmount = g.Sum(r => r.Amount)
//            })
//            .ToList();

//        foreach (var reward in groupedRewards)
//        {
//            var playerBalance = await _playerBalanceRepository.Query()
//                .FirstOrDefaultAsync(pb => pb.PlayerId == reward.PlayerId && pb.CoinId == reward.CoinId, cancellationToken);

//            if (playerBalance != null)
//            {
//                var newAmount = playerBalance.Amount + reward.TotalAmount;
//                playerBalance.SetAmount(newAmount);
//                _playerBalanceRepository.Update(playerBalance);
//            }
//            else
//            {
//                playerBalance = new PlayerBalance(reward.TotalAmount, reward.PlayerId, reward.CoinId);
//                await _playerBalanceRepository.InsertAsync(playerBalance);
//            }

//            // Log the transaction
//            await _transactionService.CreateTransactionAndApplyBalanceAsync(
//                reward.PlayerId,
//                reward.CoinId,
//                reward.TotalAmount,
//                AccountType.Casino,
//                AccountType.Player,
//                TransactionType.Reward);
//        }

//        // Save all changes in a single transaction
//        await _unitOfWork.SaveAsync();

//        return Unit.Value;
//    }
//}
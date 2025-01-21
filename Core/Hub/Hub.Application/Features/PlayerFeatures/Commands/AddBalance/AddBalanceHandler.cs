using Hub.Application.Models.Tansaction;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using System.Security.Principal;

namespace Hub.Application.Features.PlayerFeatures.Commands.AddBalance;

public class AddBalanceHandler : IRequestHandler<AddBalanceCommand, TransactionResponseModel>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ICoinRepository _coinRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerBalanceService _playerBalanceService;

    public AddBalanceHandler(
        IPlayerRepository playerRepository,
        ICoinRepository coinRepository,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork,
        IPlayerBalanceService playerBalanceService = null)
    {
        _playerRepository = playerRepository;
        _coinRepository = coinRepository;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
        _playerBalanceService = playerBalanceService;
    }

    public async Task<TransactionResponseModel> Handle(AddBalanceCommand request, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.OfIdAsync(request.PlayerId);

        if (player == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Player with the specified ID: [{request.PlayerId}] was not found.");
        }

        var coin = await _coinRepository.Query(c => c.Id == request.CoinId)
                                        .FirstOrDefaultAsync();

        if (coin == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Coin with the specified ID: [{request.CoinId}] was not found.");
        }

        await _playerBalanceService.ApplyPlayerBalanceOperationAsync(
            player.Id, 
            coin.Id, 
            AccountType.Casino,
            AccountType.Player,
            request.Amount,
            coin.PromotionId
        );

        var transaction = new Transaction(
            request.Amount,
            null,
            null,
            player.Id,
            AccountType.Casino,
            AccountType.Player, 
            coin.Id, 
            TransactionStatus.Created,
            TransactionType.Reward,
            coin.PromotionId);

        await _transactionRepository.InsertAsync(transaction);
        await _unitOfWork.SaveAsync();

        return new TransactionResponseModel
        {
            Id = transaction.Id,
            Success = true,
        };
    }
}
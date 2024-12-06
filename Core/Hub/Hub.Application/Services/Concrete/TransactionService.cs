using Hub.Application.Models.Tansaction;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Services.Concrete;

public class TransactionService : ITransactionService
{
    private readonly IAuthService _authService;
    private readonly IPlayerBalanceService _playerBalanceService;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionService(IAuthService authService, IPlayerBalanceService playerBalanceService, ITransactionRepository transactionRepository, IPlayerRepository playerRepository, IUnitOfWork unitOfWork)
    {
        _authService = authService;
        _playerBalanceService = playerBalanceService;
        _transactionRepository = transactionRepository;
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionResponseModel> CreateTransactionAndApplyBalanceAsync(int? gameId, string currencyId, decimal amount, AccountType fromAccount, AccountType toAccount, TransactionType transactionType, int? promotionId)
    {
        var playerId = _authService.GetCurrentPlayerId();

        // check and apply player balances
        await _playerBalanceService.ApplyPlayerBalanceOperationAsync(playerId, currencyId, fromAccount, toAccount, amount, promotionId);

        var player = await _playerRepository.OfIdAsync(playerId);

        if (player == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Player with the specified ID: [{playerId}] was not found.");
        }

        if (!player.HasPlayed)
            player.UpdateHasPlayed();

        var transaction = new Transaction(amount, gameId, playerId, fromAccount, toAccount, currencyId, TransactionStatus.Created, transactionType, null /* Needs Promotion id */);

        await _transactionRepository.InsertAsync(transaction);
        await _unitOfWork.SaveAsync();

        return new TransactionResponseModel
        {
            Id = transaction.Id,
            Success = true,
        };
    }

    public async Task<TransactionResponseModel> CreateLeaderboardTransactionAndApplyBalanceAsync(int? gameId, string currencyId, decimal amount, AccountType fromAccount, AccountType toAccount, TransactionType transactionType, int? promotionId, int playerId)
    {
        await _playerBalanceService.ApplyPlayerBalanceOperationAsync(playerId, currencyId, fromAccount, toAccount, amount, promotionId);

        var player = await _playerRepository.OfIdAsync(playerId);

        if (player == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Player with the specified ID: [{playerId}] was not found.");
        }

        if (!player.HasPlayed)
            player.UpdateHasPlayed();

        var transaction = new Transaction(amount, gameId, playerId, fromAccount, toAccount, currencyId, TransactionStatus.Created, transactionType, null /* Needs Promotion id */);

        await _transactionRepository.InsertAsync(transaction);
        await _unitOfWork.SaveAsync();

        return new TransactionResponseModel
        {
            Id = transaction.Id,
            Success = true,
        };
    }
}
using Hub.Application.Models.Tansaction;
using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;

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

    public async Task<TransactionResponseModel> CreateTransaction(int gameId, string currencyId, decimal amount, AccountType fromAccount, AccountType toAccount, TransactionType transactionType)
    {
        var playerId = _authService.GetCurrentPlayerId();

        // check and apply player balances
        await _playerBalanceService.ApplyPlayerBalanceOperationAsync(playerId, currencyId, fromAccount, toAccount, amount);

        var player = await _playerRepository.OfIdAsync(playerId);

        if (player == null)
            throw new Exception("Player not found");

        if (!player.HasPlayed)
            player.UpdateHasPlayed();

        var transaction = new Transaction(amount, gameId, playerId, fromAccount, toAccount, currencyId, TransactionStatus.Created, transactionType);

        await _transactionRepository.InsertAsync(transaction);
        await _unitOfWork.SaveAsync();

        return new TransactionResponseModel
        {
            Id = transaction.Id,
            Success = true,
        };
    }
}
using Hub.Application.Models.Tansaction;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;
using MediatR;
using Newtonsoft.Json;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Aggregation;

namespace Hub.Application.Services.Concrete;

public class TransactionService : ITransactionService
{
    private readonly IAuthService _authService;
    private readonly IPlayerBalanceService _playerBalanceService;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IMessageBus _messageBus;

    public TransactionService(IAuthService authService, IPlayerBalanceService playerBalanceService, ITransactionRepository transactionRepository, IPlayerRepository playerRepository, IUnitOfWork unitOfWork, IMediator mediator, IMessageBus messageBus)
    {
        _authService = authService;
        _playerBalanceService = playerBalanceService;
        _transactionRepository = transactionRepository;
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _messageBus = messageBus;
    }

    public async Task<TransactionResponseModel> CreateTransactionAndApplyBalanceAsync(int? keyId, string sourceServiceName, string coinId, decimal amount, AccountType fromAccount, AccountType toAccount, TransactionType transactionType, int promotionId)
    {
        var playerId = _authService.GetCurrentPlayerId();

        var player = await _playerRepository.OfIdAsync(playerId);

        if (player == null)
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Player with the specified ID: [{playerId}] was not found.");

        if (!player.HasPlayed)
            player.UpdateHasPlayed();

        // check and apply player balances
        await _playerBalanceService.ApplyPlayerBalanceOperationAsync(playerId, coinId, fromAccount, toAccount, amount, promotionId);

        var transaction = new Transaction(amount, keyId, sourceServiceName, playerId, fromAccount, toAccount, coinId, TransactionStatus.Created, transactionType, promotionId);

        await _transactionRepository.InsertAsync(transaction);
        await _unitOfWork.SaveAsync();

        await SendAggregationEventAsync(keyId, sourceServiceName, coinId, amount, fromAccount, toAccount, transactionType, promotionId, playerId);

        return new TransactionResponseModel
        {
            Id = transaction.Id,
            Success = true,
        };
    }

    public async Task<TransactionResponseModel> CreateLeaderboardTransactionAndApplyBalanceAsync(int? keyId, string sourceServiceName, string coinId, decimal amount, AccountType fromAccount, AccountType toAccount, TransactionType transactionType, int promotionId, int playerId)
    {
        await _playerBalanceService.ApplyPlayerBalanceOperationAsync(playerId, coinId, fromAccount, toAccount, amount, promotionId);

        var player = await _playerRepository.OfIdAsync(playerId);

        if (player == null)
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Player with the specified ID: [{playerId}] was not found.");

        if (!player.HasPlayed)
            player.UpdateHasPlayed();

        var transaction = new Transaction(amount, keyId, sourceServiceName, playerId, fromAccount, toAccount, coinId, TransactionStatus.Created, transactionType, promotionId);

        await _transactionRepository.InsertAsync(transaction);
        await _unitOfWork.SaveAsync();

        return new TransactionResponseModel
        {
            Id = transaction.Id,
            Success = true,
        };
    }

    public async Task SendAggregationEventAsync(int? keyId, string sourceServiceName, string coinId, decimal amount, AccountType fromAccount, AccountType toAccount, TransactionType transactionType, int promotionId, int playerId)
    {
        if (transactionType == TransactionType.Bet || transactionType == TransactionType.Win)
        {
            var @events = new AggregationTriggerEvent(
               data: JsonConvert.SerializeObject(new Dictionary<string, string>
               {
                            { "customerId", playerId.ToString() },
                            { "transactionType", transactionType.Name.ToString() },
                            { "timestamp", DateTime.Now.ToString() },
                            { "producer", "Hub" },
                            { "promotionId", promotionId.ToString() },
                            { "value", amount.ToString() },
                            { "deviceType" , "Mobile" },
                            { "coin", coinId }
               }),

              producer: "Hub"
            );

            await _messageBus.Publish(@events);
            Console.WriteLine(@events.Data);
        }
        //try
        //{
        //    var playerId = _authService.GetCurrentPlayerId();

        //    var player = await _playerRepository.OfIdAsync(playerId);

        //    if (player == null)
        //        throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Player with the specified ID: [{playerId}] was not found.");

        //    if (!player.HasPlayed)
        //        player.UpdateHasPlayed();

        //    await _playerBalanceService.ApplyPlayerBalanceOperationAsync(playerId, coinId, fromAccount, toAccount, amount, promotionId);

        //    var transaction = new Transaction(amount, keyId, sourceServiceName, playerId, fromAccount, toAccount, coinId, TransactionStatus.Created, transactionType, promotionId);

        //    await _transactionRepository.InsertAsync(transaction);
        //    await _unitOfWork.SaveAsync();


        //    return new TransactionResponseModel
        //    {
        //        Id = transaction.Id,
        //        Success = true,
        //    };
        //}
        //catch (Exception Ex)
        //{
        //    throw new Exception($"Test12345 + {Ex}");
        //}
    }
}
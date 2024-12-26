using Hub.Application.Models.Tansaction;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities.DbEnums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateWinTransaction;

public class CreateWinTransactionHandler : IRequestHandler<CreateWinTransactionCommand, TransactionResponseModel>
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly ITransactionService _transactionService;
    private readonly ICoinRepository _coinRepository;

    public CreateWinTransactionHandler(IPromotionRepository promotionRepository, ITransactionService transactionService, ICoinRepository coinRepository)
    {
        _promotionRepository = promotionRepository;
        _transactionService = transactionService;
        _coinRepository = coinRepository;
    }

    public async Task<TransactionResponseModel> Handle(CreateWinTransactionCommand request, CancellationToken cancellationToken)
    {
        if (!CheckmateValidations.Checkmate.IsValid(request, true))
        {
            throw new CheckmateException(CheckmateValidations.Checkmate.GetFailedChecks(request, true));
        }

        var promotion = await _promotionRepository.Query(p => p.Id == request.PromotionId)
                                                  .Include(p => p.Coins)
                                                  .FirstOrDefaultAsync();

        if (promotion == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Promotion with the specified ID: [{request.PromotionId}] was not found.");
        }

        var coin = await _coinRepository.Query(c => c.Id == request.CoinId).FirstOrDefaultAsync();
        
        if (coin == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"CoinId with the specified name: [{request.CoinId.Split('_')[1]}] was not found in [{request.PromotionId}] promotion.");
        }

        return await _transactionService.CreateTransactionAndApplyBalanceAsync(
            request.GameId,
            request.CoinId,
            request.Amount,
            AccountType.Game,
            AccountType.Player,
            TransactionType.Win,
            request.PromotionId
        );
    }
}
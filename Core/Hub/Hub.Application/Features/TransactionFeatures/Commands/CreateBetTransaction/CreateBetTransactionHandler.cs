using Hub.Application.Models.Tansaction;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities.DbEnums;
using Hub.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateBetTransaction;

public class CreateBetTransactionHandler : IRequestHandler<CreateBetTransactionCommand, TransactionResponseModel>
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly ITransactionService _transactionService;

    public CreateBetTransactionHandler(IPromotionRepository promotionRepository, ITransactionService transactionService)
    {
        _promotionRepository = promotionRepository;
        _transactionService = transactionService;
    }

    public async Task<TransactionResponseModel> Handle(CreateBetTransactionCommand request, CancellationToken cancellationToken)
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

        var inCoin = promotion.Coins.First(c => c.CoinType == CoinType.In).Id;

        return await _transactionService.CreateTransactionAndApplyBalanceAsync(
            request.GameId, 
            inCoin, 
            request.Amount, 
            AccountType.Player,
            AccountType.Game, 
            TransactionType.Bet, 
            request.PromotionId
        );
    }
}
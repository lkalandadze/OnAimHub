using Hub.Application.Models.Tansaction;
using Hub.Application.Services.Abstract;
using Hub.Domain.Entities.DbEnums;
using MediatR;
using Shared.Application.Exceptions;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateBetTransaction;

public class CreateBetTransactionHandler : IRequestHandler<CreateBetTransactionCommand, TransactionResponseModel>
{
    private readonly ITransactionService _transactionService;

    public CreateBetTransactionHandler(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<TransactionResponseModel> Handle(CreateBetTransactionCommand request, CancellationToken cancellationToken)
    {
        if (!CheckmateValidations.Checkmate.IsValid(request, true))
        {
            throw new CheckmateException(CheckmateValidations.Checkmate.GetFailedChecks(request, true));
        }

        return await _transactionService.CreateTransactionAndApplyBalanceAsync(request.GameId, request.CoinId, request.Amount,
                                                           AccountType.Player, AccountType.Game, TransactionType.Bet, request.PromotionId /* PromotionId */);
    }
}
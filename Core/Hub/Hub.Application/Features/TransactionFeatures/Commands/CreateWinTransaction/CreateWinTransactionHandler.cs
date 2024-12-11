using Hub.Application.Models.Tansaction;
using Hub.Application.Services.Abstract;
using Hub.Domain.Entities.DbEnums;
using MediatR;
using Shared.Application.Exceptions;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateWinTransaction;

public class CreateWinTransactionHandler : IRequestHandler<CreateWinTransactionCommand, TransactionResponseModel>
{
    private readonly ITransactionService _transactionService;

    public CreateWinTransactionHandler(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<TransactionResponseModel> Handle(CreateWinTransactionCommand request, CancellationToken cancellationToken)
    {
        if (!CheckmateValidations.Checkmate.IsValid(request, true))
        {
            throw new CheckmateException(CheckmateValidations.Checkmate.GetFailedChecks(request, true));
        }

        return await _transactionService.CreateTransactionAndApplyBalanceAsync(request.GameId, request.CoinId, request.Amount,
                                                           AccountType.Game, AccountType.Player, TransactionType.Win, request.PromotionId /* PromotionId */);
    }
}
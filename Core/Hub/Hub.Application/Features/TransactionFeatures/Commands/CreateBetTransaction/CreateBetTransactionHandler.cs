using Hub.Application.Models.Tansaction;
using Hub.Application.Services.Abstract;
using Hub.Domain.Entities.DbEnums;
using MediatR;

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
        return await _transactionService.CreateTransactionAndApplyBalanceAsync(request.GameId, request.CoinId, request.Amount,
                                                           AccountType.Player, AccountType.Game, TransactionType.Bet, request.PromotionId /* PromotionId */);
    }
}
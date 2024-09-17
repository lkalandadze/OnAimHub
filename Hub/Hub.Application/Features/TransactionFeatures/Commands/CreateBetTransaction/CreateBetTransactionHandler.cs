using Hub.Application.Models.Tansaction;
using Hub.Application.Services.Abstract;
using Hub.Domain.Entities;
using MediatR;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateBetTransaction;

public class CreateBetTransactionHandler : IRequestHandler<CreateBetTransaction, TransactionResponseModel>
{
    private readonly ITransactionService _transactionService;

    public CreateBetTransactionHandler(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<TransactionResponseModel> Handle(CreateBetTransaction request, CancellationToken cancellationToken)
    {
        return await _transactionService.CreateTransactionAndApplyBalanceAsync(request.GameId, request.CurrencyId, request.Amount,
                                                           AccountType.Player, AccountType.Game, TransactionType.Bet);
    }
}
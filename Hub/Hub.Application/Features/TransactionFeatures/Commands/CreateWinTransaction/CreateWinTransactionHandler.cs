using Hub.Application.Models.Tansaction;
using Hub.Application.Services.Abstract;
using Hub.Domain.Entities;
using MediatR;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateWinTransaction;

public class CreateWinTransactionHandler : IRequestHandler<CreateWinTransaction, TransactionResponseModel>
{
    private readonly ITransactionService _transactionService;

    public CreateWinTransactionHandler(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<TransactionResponseModel> Handle(CreateWinTransaction request, CancellationToken cancellationToken)
    {
        return await _transactionService.CreateTransaction(request.GameId, request.CurrencyId, request.Amount,
                                                           AccountType.Game, AccountType.Player, TransactionType.Win);
    }
}
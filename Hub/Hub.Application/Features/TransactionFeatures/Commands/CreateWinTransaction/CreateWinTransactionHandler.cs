using Hub.Application.Models.Tansaction;
using Hub.Application.Services.Abstract;
using Hub.Domain.Entities;
using MediatR;

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
        return await _transactionService.CreateTransactionAndApplyBalanceAsync(request.GameId, request.CurrencyId, request.Amount,
                                                           AccountType.Game, AccountType.Player, TransactionType.Win);
    }
}
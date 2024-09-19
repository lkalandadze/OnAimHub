using Hub.Application.Models.Tansaction;
using MediatR;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateWinTransaction;

public record CreateWinTransactionCommand(int GameId, string CurrencyId, decimal Amount) : IRequest<TransactionResponseModel>;
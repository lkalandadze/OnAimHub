using Hub.Application.Models.Tansaction;
using MediatR;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateWinTransaction;

public record CreateWinTransaction(int GameId, string CurrencyId, decimal Amount) : IRequest<TransactionResponseModel>;
using Hub.Application.Models.Tansaction;
using MediatR;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateBetTransaction;

public record CreateBetTransaction(int GameId, string CurrencyId, decimal Amount) : IRequest<TransactionResponseModel>;
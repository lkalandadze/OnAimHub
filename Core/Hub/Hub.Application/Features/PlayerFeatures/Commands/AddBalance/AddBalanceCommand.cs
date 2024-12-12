using Hub.Application.Models.Tansaction;
using MediatR;

namespace Hub.Application.Features.PlayerFeatures.Commands.AddBalance;

public record AddBalanceCommand(
    int PlayerId,
    string CoinId,
    decimal Amount) : IRequest<TransactionResponseModel>;
using Hub.Application.Models.Tansaction;
using MediatR;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateBetTransaction;

public record CreateBetTransactionCommand(int GameId, string CoinId, decimal Amount, int PromotionId) : IRequest<TransactionResponseModel>;
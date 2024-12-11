using CheckmateValidations;
using Hub.Application.Models.Tansaction;
using MediatR;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateBetTransaction;

[CheckMate<CreateBetTransactionChecker>]
public record CreateBetTransactionCommand(
    int GameId,
    string CoinId,
    decimal Amount,
    int PromotionId) : IRequest<TransactionResponseModel>;
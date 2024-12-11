using CheckmateValidations;
using Hub.Application.Models.Tansaction;
using MediatR;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateWinTransaction;

[CheckMate<CreateWinTransactionChecker>]
public record CreateWinTransactionCommand(
    int GameId,
    string CoinId,
    decimal Amount,
    int PromotionId) : IRequest<TransactionResponseModel>;
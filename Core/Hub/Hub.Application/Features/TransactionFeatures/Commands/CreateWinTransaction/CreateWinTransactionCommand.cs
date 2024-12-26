using CheckmateValidations;
using Hub.Application.Models.Tansaction;
using MediatR;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateWinTransaction;

[CheckMate<CreateWinTransactionChecker>]
public record CreateWinTransactionCommand(
    int GameId,
    decimal Amount,
    string CoinId,
    int PromotionId) : IRequest<TransactionResponseModel>;
using CheckmateValidations;
using Hub.Application.Models.Tansaction;
using MediatR;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateBetTransaction;

[CheckMate<CreateBetTransactionChecker>]
public record CreateBetTransactionCommand(
    int KeyId,
    string SourceServiceName,
    decimal Amount,
    int PromotionId) : IRequest<TransactionResponseModel>;
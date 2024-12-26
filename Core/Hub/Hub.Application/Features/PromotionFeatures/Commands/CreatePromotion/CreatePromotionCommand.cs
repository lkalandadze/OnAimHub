using CheckmateValidations;
using Hub.Application.Models.Coin;
using MediatR;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotion;

[CheckMate<CreatePromotionChecker>]
public record CreatePromotionCommand(
    string Title,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Description,
    Guid CorrelationId,
    int? CreatedByUserId,
    string? TemplateId,
    IEnumerable<string> SegmentIds,
    IEnumerable<CreateCoinModel> Coins) : IRequest<PromotionResponse>;
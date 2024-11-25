using Hub.Application.Models.PromotionCoin;
using MediatR;

namespace Hub.Application.Features.PromotionFeatures.Commands.Create;

public record CreatePromotionCommand(
    string Title,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Description,
    Guid CorrelationId,
    IEnumerable<string> SegmentIds,
    IEnumerable<CreatePromotionCoinModel> PromotionCoin) : IRequest;
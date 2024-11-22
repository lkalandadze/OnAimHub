using Hub.Application.Models.PromotionCoin;
using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.PromotionFeatures.Commands.Create;

public record CreatePromotionCommand(
    string Title,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Description,
    IEnumerable<string> SegmentIds,
    IEnumerable<CreatePromotionCoinModel> PromotionCoin) : IRequest;
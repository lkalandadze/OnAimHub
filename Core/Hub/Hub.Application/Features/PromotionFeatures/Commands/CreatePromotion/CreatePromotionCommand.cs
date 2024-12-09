using Hub.Application.Models.Coin;
using MediatR;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotion;

public record CreatePromotionCommand(
    string Title,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Description,
    Guid CorrelationId,
    string? TemplateId,
    IEnumerable<string> SegmentIds,
    IEnumerable<CreateCoinModel> Coins) : IRequest<int>;


//{
//  "title": "New Year Promotion",
//  "startDate": "2024-12-01T00:00:00Z",
//  "endDate": "2024-12-31T23:59:59Z",
//  "description": "Promotion for the new year",
//  "correlationId": "123e4567-e89b-12d3-a456-426614174000",
//  "segmentIds": ["default"],
//  "promotionCoins": [
//    {
//      "name": "Incoming Coin 1",
//      "description": "First incoming coin",
//      "imageUrl": "https://example.com/incoming1.png",
//      "coinType": 1
//    },
//    {
//    "name": "Outgoing Coin 1",
//      "description": "First outgoing coin",
//      "imageUrl": "https://example.com/outgoing1.png",
//      "coinType": 2,
//      "withdrawOptions": [
//        { "title": "withdrawOption1", "description": "First withdraw option", "imageUrl": "https://example.com/wo1.png" }
//      ]
//    },
//    {
//    "name": "Internal Coin 1",
//      "description": "First internal coin",
//      "imageUrl": "https://example.com/internal1.png",
//      "coinType": 3
//    },
//    {
//    "name": "Prize Coin 1",
//      "description": "First prize coin",
//      "imageUrl": "https://example.com/prize1.png",
//      "coinType": 4
//    }
//  ]
//}
using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.CoinFeatures.Commands.CreatePromotionCoin;

public record CreatePromotionCoin(string Name, string? Description, string ImageUrl, CoinType CoinType, int PromotionId, IEnumerable<int>? WithdrawOptionIds) : IRequest;
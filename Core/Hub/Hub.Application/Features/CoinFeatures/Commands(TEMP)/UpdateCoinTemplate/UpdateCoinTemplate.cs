using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.CoinFeatures.Commands.UpdateCoinTemplate;

public record UpdateCoinTemplate(int Id, string Name, string? Description, string ImageUrl, CoinType CoinType, IEnumerable<int>? WithdrawOptionIds) : IRequest;
using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.CoinFeatures.Commands.CreateCoinTemplate;

public record CreateCoinTemplate(string Name, string? Description, string IconUrl, CoinType CoinType, IEnumerable<int>? WithdrawOptionIds) : IRequest;
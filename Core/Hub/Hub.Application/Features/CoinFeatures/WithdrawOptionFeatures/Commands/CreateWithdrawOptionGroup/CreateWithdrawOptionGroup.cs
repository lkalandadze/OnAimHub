using MediatR;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.CreateWithdrawOptionGroup;

public record CreateWithdrawOptionGroup(
    string Title,
    string Description,
    string ImageUrl,
    int? PriorityIndex,
    IEnumerable<int>? WithdrawOptionIds,
    int? CreatedByUserId) : IRequest;
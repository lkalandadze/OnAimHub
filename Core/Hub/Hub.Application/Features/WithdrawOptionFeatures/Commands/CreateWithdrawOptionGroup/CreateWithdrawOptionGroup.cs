using MediatR;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.CreateWithdrawOptionGroup;

public record CreateWithdrawOptionGroup(
    string Title,
    string Description,
    string ImageUrl,
    int? PriorityIndex,
    IEnumerable<int>? WithdrawOptionIds) : IRequest;
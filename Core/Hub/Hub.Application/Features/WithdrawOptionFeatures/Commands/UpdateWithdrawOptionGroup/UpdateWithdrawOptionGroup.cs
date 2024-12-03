using MediatR;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.UpdateWithdrawOptionGroup;

public record UpdateWithdrawOptionGroup(
    int Id,
    string Title,
    string Description,
    string ImageUrl,
    int? PriorityIndex,
    IEnumerable<int>? WithdrawOptionIds) : IRequest;
using CheckmateValidations;
using MediatR;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.UpdateWithdrawOptionGroup;

[CheckMate<UpdateWithdrawOptionGroupChecker>]
public record UpdateWithdrawOptionGroup(
    int Id,
    string Title,
    string Description,
    string ImageUrl,
    int? PriorityIndex,
    IEnumerable<int>? WithdrawOptionIds) : IRequest;
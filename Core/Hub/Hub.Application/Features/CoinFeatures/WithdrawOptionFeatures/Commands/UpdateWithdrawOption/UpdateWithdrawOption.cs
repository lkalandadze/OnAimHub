using CheckmateValidations;
using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.UpdateWithdrawOption;

[CheckMate<UpdateWithdrawOptionChecker>]
public record UpdateWithdrawOption(
    int Id,
    string Title,
    string Description,
    string ImageUrl,
    string Endpoint,
    EndpointContentType EndpointContentType,
    string EndpointContent,
    int? WithdrawOptionEndpointId,
    IEnumerable<int>? WithdrawOptionGroupIds) : IRequest;
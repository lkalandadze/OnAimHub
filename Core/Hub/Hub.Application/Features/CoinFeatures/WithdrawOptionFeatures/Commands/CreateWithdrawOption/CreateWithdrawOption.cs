using CheckmateValidations;
using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.CreateWithdrawOption;

[CheckMate<CreateWithdrawOptionChecker>]
public record CreateWithdrawOption(
    string Title,
    string Description,
    string ImageUrl,
    decimal Value,
    string Endpoint,
    EndpointContentType
    EndpointContentType,
    string EndpointContent,
    int? WithdrawOptionEndpointId,
    IEnumerable<int>? WithdrawOptionGroupIds,
    int? CreatedByUserId) : IRequest;
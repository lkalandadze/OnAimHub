using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.UpdateWithdrawOption;

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
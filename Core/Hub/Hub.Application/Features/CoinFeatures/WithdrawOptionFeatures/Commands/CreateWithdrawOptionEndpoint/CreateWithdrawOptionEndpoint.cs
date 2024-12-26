using CheckmateValidations;
using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.CreateWithdrawOptionEndpoint;

[CheckMate<CreateWithdrawOptionEndpointChecker>]
public record CreateWithdrawOptionEndpoint(string Name, string Endpoint, string Content, EndpointContentType ContentType, int? CreatedByUserId) : IRequest;
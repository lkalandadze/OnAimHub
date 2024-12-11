using CheckmateValidations;
using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.UpdateWithdrawOptionEndpoint;

[CheckMate<UpdateWithdrawOptionEndpointChecker>]
public record UpdateWithdrawOptionEndpoint(
    int Id, 
    string Name,
    string Endpoint, 
    string Content, 
    EndpointContentType ContentType) : IRequest;
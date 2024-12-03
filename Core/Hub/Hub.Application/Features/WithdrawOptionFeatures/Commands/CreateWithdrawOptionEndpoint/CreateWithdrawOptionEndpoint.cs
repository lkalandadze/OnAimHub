using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.CreateWithdrawOptionEndpoint;

public record CreateWithdrawOptionEndpoint(string Name, string Endpoint, string Content, EndpointContentType ContentType) : IRequest;
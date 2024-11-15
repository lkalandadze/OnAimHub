using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.CreateWithdrawEndpointTemplate;

public record CreateWithdrawEndpointTemplate(string Name, string Endpoint, string Content, EndpointContentType ContentType) : IRequest;
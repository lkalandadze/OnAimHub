using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.UpdateWithdrawEndpointTemplate;

public record UpdateWithdrawEndpointTemplate(int Id, string Name, string Endpoint, string Content, EndpointContentType ContentType) : IRequest;
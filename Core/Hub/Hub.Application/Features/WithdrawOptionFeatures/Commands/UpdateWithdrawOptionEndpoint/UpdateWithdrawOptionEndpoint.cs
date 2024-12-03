using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.UpdateWithdrawOptionEndpoint;

public record UpdateWithdrawOptionEndpoint(int Id, string Name, string Endpoint, string Content, EndpointContentType ContentType) : IRequest;
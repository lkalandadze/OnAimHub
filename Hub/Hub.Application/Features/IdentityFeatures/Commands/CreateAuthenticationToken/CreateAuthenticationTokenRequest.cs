using MediatR;

namespace Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;

public record CreateAuthenticationTokenRequest(string CasinoToken)
    : IRequest<CreateAuthenticationTokenResponse>;
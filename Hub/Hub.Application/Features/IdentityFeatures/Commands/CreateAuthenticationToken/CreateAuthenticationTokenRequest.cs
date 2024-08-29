using Hub.Domain.Wrappers;
using MediatR;

namespace Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;

public record CreateAuthenticationTokenRequest(string CasinoToken)
    : IRequest<Response<CreateAuthenticationTokenResponse>>;
using MediatR;
using Shared.Domain.Wrappers;

namespace Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;

public record CreateAuthenticationTokenRequest(string CasinoToken)
    : IRequest<Response<CreateAuthenticationTokenResponse>>;
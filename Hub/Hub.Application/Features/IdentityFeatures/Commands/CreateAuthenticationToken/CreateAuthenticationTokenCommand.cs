using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;

public record CreateAuthenticationTokenCommand(string CasinoToken, string? PromoCode)
    : IRequest<Response<CreateAuthenticationTokenResponse>>;
using MediatR;

namespace Hub.Application.Features.IdentityFeatures.Commands.RefreshTokens;

public record RefreshTokensCommand(string AccessToken, string RefreshToken)
    : IRequest<RefreshTokensCommandResponse>;
using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.IdentityFeatures.Commands.RefreshTokens;

public record RefreshTokensCommand(string AccessToken, string RefreshToken)
    : IRequest<Response<RefreshTokensCommandResponse>>;
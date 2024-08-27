using Hub.Application.Services.Abstract;
using MediatR;

namespace Hub.Application.Features.IdentityFeatures.Commands.RefreshTokens;

public class RefreshTokensCommandHandler : IRequestHandler<RefreshTokensCommand, RefreshTokensCommandResponse>
{
    private readonly ITokenService _tokenService;
    public RefreshTokensCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<RefreshTokensCommandResponse> Handle(RefreshTokensCommand command, CancellationToken cancellationToken)
    {
        (string newAccessToken, string newRefreshToken) = await _tokenService.RefreshAccessTokenAsync(command.AccessToken, command.RefreshToken);

        return new RefreshTokensCommandResponse(true, newAccessToken, newRefreshToken);
    }
}
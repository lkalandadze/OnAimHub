using Hub.Application.Services.Abstract;
using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.IdentityFeatures.Commands.RefreshTokens;

public class RefreshTokensCommandHandler : IRequestHandler<RefreshTokensCommand, Response<RefreshTokensCommandResponse>>
{
    private readonly ITokenService _tokenService;
    public RefreshTokensCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<Response<RefreshTokensCommandResponse>> Handle(RefreshTokensCommand command, CancellationToken cancellationToken)
    {
        (string newAccessToken, string newRefreshToken) = await _tokenService.RefreshAccessTokenAsync(command.AccessToken, command.RefreshToken);

        var response = new RefreshTokensCommandResponse(newAccessToken, newRefreshToken);
        return new Response<RefreshTokensCommandResponse>(response);
    }
}
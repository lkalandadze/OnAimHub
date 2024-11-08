namespace Hub.Application.Features.IdentityFeatures.Commands.RefreshTokens;

public record RefreshTokensCommandResponse(string AccessToken, string RefreshToken);
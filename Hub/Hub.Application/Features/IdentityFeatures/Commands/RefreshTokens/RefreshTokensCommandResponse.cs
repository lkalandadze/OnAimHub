namespace Hub.Application.Features.IdentityFeatures.Commands.RefreshTokens;

public record RefreshTokensCommandResponse(bool Success, string AccessToken, string RefreshToken);
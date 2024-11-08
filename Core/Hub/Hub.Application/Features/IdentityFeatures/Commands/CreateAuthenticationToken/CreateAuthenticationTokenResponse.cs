namespace Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;

public record CreateAuthenticationTokenResponse(string AccessToken, string RefreshToken);
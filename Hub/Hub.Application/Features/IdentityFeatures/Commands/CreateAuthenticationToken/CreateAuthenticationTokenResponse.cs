namespace Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;
public record CreateAuthenticationTokenResponse(bool Success, string AccessToken, string RefreshToken);
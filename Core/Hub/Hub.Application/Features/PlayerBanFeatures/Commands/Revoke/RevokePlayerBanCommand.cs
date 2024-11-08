using MediatR;

namespace Hub.Application.Features.PlayerBanFeatures.Commands.Revoke;

public record RevokePlayerBanCommand(int Id) : IRequest;
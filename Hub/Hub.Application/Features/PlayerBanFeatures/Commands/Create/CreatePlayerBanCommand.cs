using MediatR;

namespace Hub.Application.Features.PlayerBanFeatures.Commands.Create;

public record CreatePlayerBanCommand(int PlayerId, DateTimeOffset? ExpireDate, bool IsPermanent, string Description) : IRequest;
using MediatR;

namespace Hub.Application.Features.PlayerBanFeatures.Commands.Update;

public record UpdatePlayerBanCommand(int Id, DateTimeOffset? ExpireDate, bool IsPermanent, string Description) : IRequest;
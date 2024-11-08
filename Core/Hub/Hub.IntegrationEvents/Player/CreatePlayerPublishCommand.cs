using MediatR;
using Shared.Application.Commands;

namespace Hub.IntegrationEvents.Player;

public record CreatePlayerPublishCommand(int PlayerId, string UserName): IRequest<CommandResult>;

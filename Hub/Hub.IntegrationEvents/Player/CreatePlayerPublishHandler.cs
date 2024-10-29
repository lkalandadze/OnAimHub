using Hub.Domain.Absractions.Repository;
using Hub.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Commands;
using Shared.Infrastructure.Bus;

using Shared.IntegrationEvents.Interfaces;

namespace Hub.IntegrationEvents.Player;
public class CreatePlayerPublishHandler : BaseCommandHandler, IRequestHandler<CreatePlayerPublishCommand, CommandResult>
{
    private readonly IPlayerRepository _playerRepository;

    public CreatePlayerPublishHandler(IMessageBus messageBus, IPlayerRepository playerRepository) : base(messageBus)
    {
        _playerRepository = playerRepository;
    }
    public async Task<CommandResult> Handle(CreatePlayerPublishCommand request, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.Query().FirstOrDefaultAsync(x => x.Id == request.PlayerId);


        var @event = new CreatePlayerEvent(Guid.NewGuid(), request.PlayerId, request.UserName);

        await PublishIntegrationEventAsync(@event);

        return CommandResult.Create(request.PlayerId.ToString());
    }
}
public record PlayerAdded(int PlayerId, string UserName, Guid CorrelationId) : IIntegrationEvent;

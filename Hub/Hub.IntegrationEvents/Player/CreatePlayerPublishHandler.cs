using Hub.Domain.Absractions.Repository;
using Hub.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Commands;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Player;

//using Shared.IntegrationEvents.IntegrationEvents.Player;
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

        //var added = MapToCommentAddedIntegrationEvent(player);

        var @event = new CreatePlayerEvent(new Guid(), request.PlayerId, request.UserName);

        await PublishIntegrationEventAsync(@event);

        return CommandResult.Create(request.PlayerId.ToString());
    }
    //private PlayerAdded MapToCommentAddedIntegrationEvent(IReadOnlyCollection<DomainEvent> domainEvents)
    //{
    //    var addedDomainEvent = (CreatePlayerEvent)domainEvents.FirstOrDefault(e => e.EventType == nameof(CreatePlayerEvent));

    //    return new PlayerAdded(addedDomainEvent.PlayerId, addedDomainEvent.UserName, addedDomainEvent.CorrelationId);
    //}
}
public record PlayerAdded(int PlayerId, string UserName, Guid CorrelationId) : IIntegrationEvent;

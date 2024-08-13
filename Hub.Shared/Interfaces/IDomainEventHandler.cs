using MediatR;

namespace Hub.Shared.Interfaces;

public interface IDomainEventHandler<T> : INotificationHandler<T>
        where T : INotification
{
    new Task Handle(T @event, CancellationToken cancellationToken);
}
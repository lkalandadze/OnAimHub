using MassTransit;

namespace Wheel.Shared.Interfaces;

public interface IIntegrationEventHandler<T> : IConsumer<T>
    where T : class, IIntegrationEvent
{

}

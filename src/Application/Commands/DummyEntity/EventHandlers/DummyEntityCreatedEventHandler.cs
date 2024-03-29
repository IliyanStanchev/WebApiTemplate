using Microsoft.Extensions.Logging;
using WebApiTemplate.Domain.Events;

namespace WebApiTemplate.Application.Commands.DummyEntity.EventHandlers;

public class DummyEntityCreatedEventHandler(ILogger<DummyEntityCreatedEventHandler> logger) : INotificationHandler<DummyEntityCreatedEvent>
{
    public Task Handle(DummyEntityCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("WebApiTemplate Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}

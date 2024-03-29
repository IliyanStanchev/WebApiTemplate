namespace WebApiTemplate.Domain.Events;

public class DummyEntityCreatedEvent(DummyEntity item) : BaseEvent
{
    public DummyEntity Item { get; } = item;
}

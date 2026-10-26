namespace BuildingBlocks.Application.Primitives.Events;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    protected DomainEvent() { }
}

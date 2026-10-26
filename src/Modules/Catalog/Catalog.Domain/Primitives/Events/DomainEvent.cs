namespace Catalog.Domain.Primitives;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    protected DomainEvent() { }
}

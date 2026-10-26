using BuildingBlocks.Application.Primitives.Events;

namespace BuildingBlocks.Domain.Bases;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}

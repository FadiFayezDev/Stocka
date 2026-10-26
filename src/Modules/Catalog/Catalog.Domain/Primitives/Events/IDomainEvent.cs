namespace Catalog.Domain.Primitives;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

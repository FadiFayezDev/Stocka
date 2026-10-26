namespace Core.Domain.Primitives;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

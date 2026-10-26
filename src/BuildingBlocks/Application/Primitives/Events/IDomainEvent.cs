namespace BuildingBlocks.Application.Primitives.Events;
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

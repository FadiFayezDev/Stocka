using BuildingBlocks.Application.Primitives.Events;
using MediatR;

namespace Inventory.Application.Primitives.Events
{
    /// <summary>
    /// MediatR envelope around a domain event.
    /// BuildingBlocks stays independent of MediatR: handlers register as
    /// INotificationHandler&lt;DomainEventNotification&lt;TEvent&gt;&gt;.
    /// </summary>
    public sealed record DomainEventNotification<TEvent>(TEvent DomainEvent) : INotification
        where TEvent : IDomainEvent;
}
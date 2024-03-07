namespace Platform.Infrastructure.Core.Domain
{
    using System;
    using Platform.Infrastructure.Core.Bus;
    using Platform.Infrastructure.Core.Events;

    /// <summary>Domain Event Interface.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Events.IEvent" />
    public interface IDomainEvent : IEvent, IBusTopicMessage
    {
        Guid Id { get; set; }

        Guid AggregateRootId { get; set; }

        int AggregateRootVersion { get; set; }
    }
}

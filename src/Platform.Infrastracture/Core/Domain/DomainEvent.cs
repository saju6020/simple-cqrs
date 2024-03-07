namespace Platform.Infrastructure.Core.Domain
{
    using System;
    using Platform.Infrastructure.Core.Events;

    /// <summary>Domain event abstraction.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Events.Event" />
    /// <seealso cref="Platform.Infrastructure.Core.Domain.IDomainEvent" />
    public abstract class DomainEvent : Event, IDomainEvent
    {
        protected DomainEvent()
        {
            this.Id = Guid.NewGuid();
        }

        private DomainEvent(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; }

        public Guid AggregateRootId { get; set; }

        public int AggregateRootVersion { get; set; }
    }
}

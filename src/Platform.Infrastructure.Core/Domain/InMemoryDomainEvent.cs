namespace Platform.Infrastructure.Core.Domain
{
    using System;
    using Platform.Infrastructure.Core.Events;

    public abstract class InMemoryDomainEvent : InMemoryEvent, IDomainEvent
    {
        protected InMemoryDomainEvent()
        {
            this.Id = Guid.NewGuid();
        }

        private InMemoryDomainEvent(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; }

        public Guid AggregateRootId { get; set; }

        public int AggregateRootVersion { get; set; }
        public string TopicName { get; set; }
    }
}

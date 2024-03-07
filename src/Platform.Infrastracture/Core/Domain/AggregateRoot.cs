namespace Platform.Infrastructure.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using Platform.Infrastructure.Domain;

    /// <summary>Aggregate root abstraction.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Domain.IAggregateRoot" />
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        public int Version { get; set; }

        private readonly List<IDomainEvent> events = new List<IDomainEvent>();

        public ReadOnlyCollection<IDomainEvent> Events => this.events.AsReadOnly();

        protected AggregateRoot()
        {
            this.Id = Guid.NewGuid();
        }

        protected AggregateRoot(Guid id)
        {
            if (id == Guid.Empty)
            {
                id = Guid.NewGuid();
            }

            this.Id = id;
        }

        public void LoadsFromHistory(IEnumerable<IDomainEvent> events)
        {
            var domainEvents = events as IDomainEvent[] ?? events.ToArray();
            if (domainEvents.Any())
            {
                this.Id = domainEvents.First().AggregateRootId;
            }

            foreach (var @event in domainEvents)
            {
                this.ApplyEvent(@event);
            }
        }

        protected void AddEvent(IDomainEvent @event)
        {
            this.events.Add(@event);
        }

        private void ApplyEvent(IDomainEvent @event)
        {
            MethodInfo methodInfo = this.GetType().GetMethod("Apply", BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Any, new Type[] { @event.GetType() }, null);

            methodInfo.Invoke(this, new[] { @event });

            // this.AsDynamic().Apply(@event);
            this.Version++;
        }

        protected void AddAndApplyEvent<T>(IDomainEvent @event)
            where T : IAggregateRoot
        {
            @event.Source = typeof(T).FullName;
            @event.TimeStamp = DateTime.UtcNow;
            this.AddEvent(@event);
            this.ApplyEvent(@event);
            @event.AggregateRootId = this.Id;
            @event.AggregateRootVersion = this.Version;
        }
    }
}

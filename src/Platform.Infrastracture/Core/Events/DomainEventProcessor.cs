namespace Platform.Infrastructure.Core.Events
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core.Commands;
    using Platform.Infrastructure.Core.Domain;


    /// <summary>Domain Event Processor to process all events and store in EventStore.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Events.IDomainEventProcessor" />
    public class DomainEventProcessor : IDomainEventProcessor
    {
        private readonly IDomainStore domainStore;
        private readonly IEventPublisher eventPublisher;
        private readonly UserContext userContext;

        public DomainEventProcessor(IDomainStore domainStore, IEventPublisher eventPublisher, UserContext userContext)
        {
            this.domainStore = domainStore;
            this.eventPublisher = eventPublisher;
            this.userContext = userContext;
        }

        public async Task Process(IEnumerable<IEvent> events, ICommand command)
        {
            try
            {
                if (events == null || !events.Any())
                {
                    return;
                }

                var domainEvents = (IEnumerable<IDomainEvent>)events;
                await this.Store(domainEvents);
                await this.PublishToBus(domainEvents, command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task Store(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                if (@event.UserContext == null)
                {
                    @event.SetUserContext(this.userContext);
                }
            }

            var aggregateDetail = events.First();
            return this.domainStore.SaveAsync(aggregateDetail.AggregateRootId, events);
        }

        public async Task PublishToBus(IEnumerable<IDomainEvent> events, ICommand command)
        {           

            foreach (var @event in events)
            {
                await this.eventPublisher.PublishAsync(@event);
            }
        }
    }
}

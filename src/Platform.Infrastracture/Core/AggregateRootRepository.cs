using Platform.Infrastructure.Core.Domain;

namespace Platform.Infrastructure.Core
{
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core.Bus;
    using Platform.Infrastructure.Core.Events;
    using Platform.Infrastructure.CustomException;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class AggregateRootRepository<T> : IAggregateRootRepository<T>
        where T : AggregateRoot
    {
        private readonly IBusMessageDispatcher busMessageDispatcher;
        private readonly IDispatcher dispatcher;
        private readonly IStateRepository stateRepository;
        private readonly IEventRepository eventRepository;
        private readonly IUserContextProvider userContextProvider;

        public AggregateRootRepository(
            IBusMessageDispatcher busMessageDispatcher,
            IDispatcher dispatcher,
            IStateRepository stateRepository,
            IEventRepository eventRepository,
            IUserContextProvider userContextProvider)
        {
            this.busMessageDispatcher = busMessageDispatcher;
            this.stateRepository = stateRepository;
            this.eventRepository = eventRepository;
            this.userContextProvider = userContextProvider;
            this.dispatcher = dispatcher;
        }

        public async Task SaveAsync(T aggregate, EventPublishOption eventPublishOption = EventPublishOption.QueuePublishOnly)
        {
            EnrichEvents(aggregate, aggregate.Events);

            if (aggregate.Events.Any(@event => @event is BusinessRuleViolatedEvent) == false)
            {
                UserContext userContext = userContextProvider.GetUserContext();
                aggregate.SetDefaultValue(userContext);

                await stateRepository.CreateAsync(aggregate);
                await SaveEventsAsync(aggregate.Events);
            }

            await PublishEventsAsync(aggregate.Events);
        }

        public async Task UpdateAsync(T aggregate, EventPublishOption eventPublishOption = EventPublishOption.QueuePublishOnly)
        {
            EnrichEvents(aggregate, aggregate.Events);

            if (aggregate.Events.Any(@event => @event is BusinessRuleViolatedEvent) == false)
            {
                UserContext userContext = userContextProvider.GetUserContext();
                aggregate.SetDefaultValue(userContext);

                await stateRepository.ReplaceAsync(aggregate, agg => agg.Id == aggregate.Id);

                await SaveEventsAsync(aggregate.Events);
            }

            await PublishEventsAsync(aggregate.Events);
        }

        public async Task UpdateAsync(T aggregate, int expectedVersion, EventPublishOption eventPublishOption = EventPublishOption.QueuePublishOnly)
        {
            EnrichEvents(aggregate, aggregate.Events);

            if (aggregate.Events.Any(@event => @event is BusinessRuleViolatedEvent) == false)
            {
                var replaced = await stateRepository.ReplaceAsync(aggregate, agg => agg.Id == aggregate.Id && agg.Version == expectedVersion);

                if (!replaced)
                {
                    throw new ConcurrencyException(aggregate.Id, expectedVersion, aggregate.Version);
                }

                await SaveEventsAsync(aggregate.Events);
            }

            await PublishEventsAsync(aggregate.Events);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return stateRepository.GetItemAsync<T>(aggregateRoot => aggregateRoot.Id == id);
        }

        public Task<T> GetByFilterAsync(Expression<Func<T, bool>> dataFilters)
        {
            return stateRepository.GetItemAsync(dataFilters);
        }

        public Task<bool> ExistAsync(Expression<Func<T, bool>> dataFilters)
        {
            return stateRepository.ExistAsync(dataFilters);
        }

        private void EnrichEvents(AggregateRoot aggregateRoot, IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            UserContext userContext = userContextProvider.GetUserContext();

            foreach (var domainEvent in domainEvents)
            {
                domainEvent.TimeStamp = DateTime.UtcNow;
                domainEvent.Source = typeof(T).FullName;
                domainEvent.SetUserContext(userContext);
            }
        }

        private async Task SaveEventsAsync<T1>(IEnumerable<T1> domainEvents) where T1 : IDomainEvent
        {
            foreach (var domainEvent in domainEvents)
            {
                var eventDocument = new EventDocument()
                {
                    AggregateId = domainEvent.AggregateRootId,
                    AggregateType = domainEvent.Source,
                    Id = Guid.NewGuid(),
                    Data = domainEvent,
                    Sequence = domainEvent.AggregateRootVersion,
                    TimeStamp = DateTime.UtcNow,
                    Type = domainEvent.GetType().FullName,
                    UserId = domainEvent.UserContext.UserId,
                };

                await eventRepository.CreateAsync(eventDocument);
            }
        }

        private async Task PublishEventsAsync(IEnumerable<IDomainEvent> domainEvents, EventPublishOption eventPublishOption = EventPublishOption.QueuePublishOnly)
        {
            foreach (var domainEvent in domainEvents)
            {
                if(eventPublishOption == EventPublishOption.DoNotPublish)
                {
                    continue;
                }
                if (eventPublishOption == EventPublishOption.InMemoryPublishOnly)
                {
                    await dispatcher.PublishAsync(domainEvent);
                }
                else if (eventPublishOption == EventPublishOption.QueuePublishOnly)
                {
                    await busMessageDispatcher.PublishAsync(domainEvent);
                }
                else if(eventPublishOption == EventPublishOption.InMemoryAndQueuePublish)
                {
                    await dispatcher.PublishAsync(domainEvent);
                    await busMessageDispatcher.PublishAsync(domainEvent);
                }
            }
        }
    }
}

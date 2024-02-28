namespace Platform.Infrastructure.Core.Domain
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
        private readonly IStateRepository stateRepository;
        private readonly IEventRepository eventRepository;
        private readonly IUserContextProvider userContextProvider;

        public AggregateRootRepository(IBusMessageDispatcher busMessageDispatcher, IStateRepository stateRepository, IEventRepository eventRepository, IUserContextProvider userContextProvider)
        {
            this.busMessageDispatcher = busMessageDispatcher;
            this.stateRepository = stateRepository;
            this.eventRepository = eventRepository;
            this.userContextProvider = userContextProvider;
        }

        public async Task SaveAsync(T aggregate)
        {
            this.EnrichEvents(aggregate, aggregate.Events);

            if (aggregate.Events.Any(@event => @event is BusinessRuleViolatedEvent) == false)
            {
                UserContext userContext = this.userContextProvider.GetUserContext();
                aggregate.SetDefaultValue(userContext);

                await this.stateRepository.CreateAsync(aggregate);
                await this.SaveEventsAsync(aggregate.Events);
            }

            await this.PublishEventsAsync(aggregate.Events);
        }

        public async Task UpdateAsync(T aggregate)
        {
            this.EnrichEvents(aggregate, aggregate.Events);

            if (aggregate.Events.Any(@event => @event is BusinessRuleViolatedEvent) == false)
            {
                UserContext userContext = this.userContextProvider.GetUserContext();
                aggregate.SetDefaultValue(userContext);

                await this.stateRepository.ReplaceAsync(aggregate, agg => agg.Id == aggregate.Id);

                await this.SaveEventsAsync(aggregate.Events);
            }

            await this.PublishEventsAsync(aggregate.Events);
        }

        public async Task UpdateAsync(T aggregate, int expectedVersion)
        {
            this.EnrichEvents(aggregate, aggregate.Events);

            if (aggregate.Events.Any(@event => @event is BusinessRuleViolatedEvent) == false)
            {
                var replaced = await this.stateRepository.ReplaceAsync(aggregate, agg => agg.Id == aggregate.Id && agg.Version == expectedVersion);

                if (!replaced)
                {
                    throw new ConcurrencyException(aggregate.Id, expectedVersion, aggregate.Version);
                }

                await this.SaveEventsAsync(aggregate.Events);
            }

            await this.PublishEventsAsync(aggregate.Events);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return this.stateRepository.GetItemAsync<T>(aggregateRoot => aggregateRoot.Id == id);
        }

        public Task<T> GetByFilterAsync(Expression<Func<T, bool>> dataFilters)
        {
            return this.stateRepository.GetItemAsync<T>(dataFilters);
        }

        public Task<bool> ExistAsync(Expression<Func<T, bool>> dataFilters)
        {
            return this.stateRepository.ExistAsync<T>(dataFilters);
        }

        private void EnrichEvents(AggregateRoot aggregateRoot, IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            UserContext userContext = this.userContextProvider.GetUserContext();

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

                await this.eventRepository.CreateAsync(eventDocument);
            }
        }

        private async Task PublishEventsAsync(IEnumerable<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                await this.busMessageDispatcher.PublishAsync(domainEvent);
            }
        }
    }
}

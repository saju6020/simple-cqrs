namespace Platform.Infrastructure.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>Generic domain repository.</summary>
    /// <typeparam name="T">Generic type.</typeparam>
    /// <seealso cref="Platform.Infrastructure.Core.Domain.IDomainRepository{T}" />
    public class DomainRepository<T> : IDomainRepository<T>
        where T : IAggregateRoot
    {
        private readonly IDomainStore domainStore;

        public DomainRepository(IDomainStore domainStore)
        {
            this.domainStore = domainStore;
        }

        public Task SaveAsync(T aggregate)
        {
            return this.domainStore.SaveAsync(aggregate.Id, aggregate.Events);
        }

        public void Save(T aggregate)
        {
            this.domainStore.Save(aggregate.Id, aggregate.Events);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var events = await this.domainStore.GetEventsAsync(id);
            var domainEvents = events as DomainEvent[] ?? events.ToArray();
            if (!domainEvents.Any())
            {
                return default;
            }

            var aggregate = Activator.CreateInstance<T>();
            aggregate.LoadsFromHistory(domainEvents);
            return aggregate;
        }

        public T GetById(Guid id)
        {
            var events = this.domainStore.GetEvents(id);
            var domainEvents = events as DomainEvent[] ?? events.ToArray();
            if (!domainEvents.Any())
            {
                return default;
            }

            var aggregate = Activator.CreateInstance<T>();
            aggregate.LoadsFromHistory(domainEvents);
            return aggregate;
        }
    }
}

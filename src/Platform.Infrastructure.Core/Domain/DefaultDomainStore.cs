namespace Platform.Infrastructure.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Platform.Infrastructure.Core.Domain;

    public class DefaultDomainStore : IDomainStore
    {
        public void Save(Guid aggregateRootId, IEnumerable<IDomainEvent> events)
        {
        }

        public Task SaveAsync(Guid aggregateRootId, IEnumerable<IDomainEvent> events)
        {
            return Task.CompletedTask;
        }

        public IEnumerable<IDomainEvent> GetEvents(Guid aggregateId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDomainEvent>> GetEventsAsync(Guid aggregateId)
        {
            throw new NotImplementedException();
        }
    }
}

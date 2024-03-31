namespace Platform.Infrastructure.Core.Domain
{
    using Platform.Infrastructure.Core.Domain;
    using System;
    using System.Threading.Tasks;
    using System.Linq.Expressions;

    public interface IAggregateRootRepository<T>
        where T : AggregateRoot
    {
        Task SaveAsync(T aggregate, EventPublishOption eventPublishOption = EventPublishOption.QueuePublishOnly);

        Task UpdateAsync(T aggregate, EventPublishOption eventPublishOption = EventPublishOption.QueuePublishOnly);

        Task UpdateAsync(T aggregate, int expectedVersion, EventPublishOption eventPublishOption = EventPublishOption.QueuePublishOnly);

        Task<T> GetByIdAsync(Guid id);

        Task<T> GetByFilterAsync(Expression<Func<T, bool>> dataFilters);

        Task<bool> ExistAsync(Expression<Func<T, bool>> dataFilters);
    }
}
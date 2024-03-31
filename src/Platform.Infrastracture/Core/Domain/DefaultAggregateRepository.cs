
/*namespace Platform.Infrastructure.Core.Domain
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
   

    /// <summary>Generic domain repository.</summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Platform.Infrastructure.Core.Domain.IDomainRepository{T}" />
    public class DefaultAggregateRepository<T> : IAggregateRootRepository<T>
        where T : IAggregateRoot
    {
        private readonly IRepository repository;
        private readonly IDispatcher dispatcher;

        public DefaultAggregateRepository(IRepository repository, IDispatcher dispatcher)
        {
            this.repository = repository;
            this.dispatcher = dispatcher;
        }

        public Task SaveAsync(T aggregate)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T aggregate)
        {
            throw new NotImplementedException();
        }

        public T GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T aggregate, int expectedVersion)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByFilterAsync(Expression<Func<T, bool>> dataFilters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistAsync(Expression<Func<T, bool>> dataFilters)
        {
            throw new NotImplementedException();
        }
    }
}*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Infrastructure.Repository.Contract
{
   public interface IRepository: IReadOnlyRepository
    {
        Task CreateAsync<TEntity>(TEntity entity)
            where TEntity : class;

        void Create<TEntity>(TEntity entity)
           where TEntity : class;

        Task UpdateAsync<TEntity>(TEntity entity)
            where TEntity : class;

        void Update<TEntity>(TEntity entity)
           where TEntity : class;

        Task DeleteAsync<TEntity>(object id)
            where TEntity : class;

        void Delete<TEntity>(TEntity entity)
            where TEntity : class;

        void Save();

        Task SaveAsync();
    }
}

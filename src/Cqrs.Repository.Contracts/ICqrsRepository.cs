namespace Platform.Infrastructure.Cqrs.Repository.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface ICqrsRepository
    {
        Task DeleteAsync<T>(Expression<Func<T, bool>> dataFilters);

        Task<T> GetItemAsync<T>(Expression<Func<T, bool>> dataFilters);

        IQueryable<T> GetItems<T>();

        IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters);

        Task SaveAsync<T>(T data);

        Task UpdateAsync<T>(Expression<Func<T, bool>> dataFilters, T data);

        Task<bool> ExistAsync<T>(Expression<Func<T, bool>> dataFilters);

        Task<bool> ReplaceAsync<T>(Expression<Func<T, bool>> dataFilters, T data);

        Task UpsertAsync<T>(Expression<Func<T, bool>> dataFilters, T data);

        Task SaveManyAsync<T>(IEnumerable<T> data);

        Task DeleteManyAsync<T>(Expression<Func<T, bool>> dataFilters);        
    }

}

namespace Platform.Infrastructure.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository
    {
        Task DeleteAsync<T>(Expression<Func<T, bool>> dataFilters) where T:class;

        Task<T> GetItemAsync<T>(Expression<Func<T, bool>> dataFilters) where T: class;

        IQueryable<T> GetItems<T>() where T: class;

        IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters) where T: class;

        Task CreateAsync<T>(T data) where T: class;

        Task UpdateAsync<T>(T data, Expression<Func<T, bool>> dataFilters) where T:class;        

        Task<bool> ExistAsync<T>(Expression<Func<T, bool>> dataFilters) where T: class;

        Task<bool> ReplaceAsync<T>(T data, Expression<Func<T, bool>> dataFilters) where T: class;

        Task UpsertAsync<T>(T data, Expression<Func<T, bool>> dataFilters) where T: class;

        Task SaveManyAsync<T>(IEnumerable<T> data) where T: class;

        Task DeleteManyAsync<T>(Expression<Func<T, bool>> dataFilters) where T:class;

        Task<IEnumerable<T>> GetItemsAsync<T>(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null,
            int? skip = null,
            int? take = null) 
            where T : class;
       
        Task<T> GetOneAsync<T>(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null)
            where T : class;                   
      
        Task<int> GetCountAsync<T>(Expression<Func<T, bool>>? filter = null)
            where T : class;
      
    }

}

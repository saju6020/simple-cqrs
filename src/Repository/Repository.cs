using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Platform.Infrastructure.Core;
using Platform.Infrastructure.Domain;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Platform.Infrastructure.Repository
{
    public class Repository<TContext> : IRepository
     where TContext : DbContext
    {
        public ChangeTracker ChangeTracker { get; set; }

        protected readonly TContext context;
        public Repository(TContext context)            
        {
            this.context = context;
        }

        public async Task CreateAsync<T>(T data)
            where T : class
        {           
           await context.Set<T>().AddAsync(data);
        }
       

        public async Task UpdateAsync<T>(T data, Expression<Func<T, bool>>? dataFilters = null)
           where T : class
        {
            await Task.Run(() =>
            {
                context.Set<T>().Attach(data);
                context.Entry(data).State = EntityState.Modified;
            });
            
        }

        public async Task DeleteAsync<T>(Guid id)
           where T : class
        {          
               T entity = await context.Set<T>().FindAsync(id);
               context.Set<T>().Remove(entity);

        }            

        public async Task SaveAsync()
        {
            try
            {

                await context.SaveChangesAsync();
                 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task DeleteAsync<T>(Expression<Func<T, bool>> dataFilters)
            where T : class
        {                      
            await Task.Run(async () =>
            {
                var item = await GetQueryable<T>(dataFilters).FirstOrDefaultAsync();
                context.Set<T>().Remove(item);
            });
        }

        public async Task<T> GetItemAsync<T>(Expression<Func<T, bool>> dataFilters)
            where T : class
        {
            return await GetQueryable<T>(dataFilters).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetItems<T>()
            where T : class
        {
            return await GetQueryable<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetItems<T>(Expression<Func<T, bool>> dataFilters)
            where T: class
        {
            return await GetQueryable<T>(dataFilters).ToListAsync();
        }
      

       

        protected IQueryable<T> GetQueryable<T>(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null,
            int? skip = null,
            int? take = null)
            where T : class
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<T> query = context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        IQueryable<T> IRepository.GetItems<T>()
        {
            return GetQueryable<T>();
        }

        IQueryable<T> IRepository.GetItems<T>(Expression<Func<T, bool>> dataFilters)
            where T : class
        {
            return GetQueryable<T>(dataFilters);
        }

        public async Task<bool> ExistAsync<T>(Expression<Func<T, bool>> dataFilters)
            where T : class
        {
            return await GetQueryable<T>(dataFilters).AnyAsync();
        }

        public async Task<bool> ReplaceAsync<T>(T data, Expression<Func<T, bool>>? dataFilters = null)
            where T : class
        {
            var item = await GetQueryable<T>(dataFilters).FirstOrDefaultAsync();
            Replace(item, data);
            return context.Entry(data).State == EntityState.Modified;
        }

        public async Task UpsertAsync<T>(T data, Expression<Func<T, bool>>? dataFilters = null)
            where T : class
        {
            await Task.Run(async () =>
            {
                context.Set<T>().Update(data);
            });
        }

        public async Task SaveManyAsync<T>(IEnumerable<T> data)
            where T : class
        {
           await context.Set<T>().AddRangeAsync(data);
        }

        public async Task DeleteManyAsync<T>(Expression<Func<T, bool>> dataFilters)
            where T : class
        {
            await Task.Run(async () =>
            {
                var items = await GetItemAsync<T>(dataFilters);
                context.Set<T>().RemoveRange(items);
            });
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null, int? skip = null, int? take = null) where T : class
        {
            return await GetQueryable<T>(filter, orderBy, includeProperties, skip, take).ToListAsync();
        }

        public async Task<T> GetOneAsync<T>(Expression<Func<T, bool>>? filter = null, string? includeProperties = null) where T : class
        {
           return await GetQueryable<T>(filter).SingleOrDefaultAsync();
        }

        public async Task<int> GetCountAsync<T>(Expression<Func<T, bool>>? filter = null) where T : class
        {
            return await GetQueryable<T>(filter).CountAsync();
        }

        public void Replace<T>(T oldEntity, T newEntity) where T : class
        {
            ChangeTracker.TrackGraph(oldEntity, e => e.Entry.State = EntityState.Deleted);
            ChangeTracker.TrackGraph(newEntity, e => e.Entry.State = e.Entry.IsKeySet ? EntityState.Modified : EntityState.Added);
        }
    }
}

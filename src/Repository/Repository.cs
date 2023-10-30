using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Platform.Infrastructure.Repository.Contract;


namespace Platform.Infrastructure.Repository
{
    public class Repository<TContext> : ReadRepository<TContext>, IRepository
     where TContext : DbContext
    {
        public ChangeTracker ChangeTracker { get; set; }
        public Repository(TContext context)
            : base(context)
        {           
        }

        public void Create<TEntity>(TEntity entity)
            where TEntity : class
        {
            context.Set<TEntity>().Add(entity);
        }

        public async Task CreateAsync<TEntity>(TEntity entity)
            where TEntity : class
        {           
           await context.Set<TEntity>().AddAsync(entity);
        }
        

        public void Update<TEntity>(TEntity entity)
            where TEntity : class
        {           
            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }


        public async Task UpdateAsync<TEntity>(TEntity entity)
           where TEntity : class
        {
            await Task.Run(() =>
            {
                context.Set<TEntity>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            });
            
        }

        public void Delete<TEntity>(object id)
            where TEntity : class
        {
            TEntity entity = context.Set<TEntity>().Find(id);
            Delete<TEntity>(entity);
        }

        public async Task DeleteAsync<TEntity>(Guid id)
           where TEntity : class
        {
           await Task.Run(() =>
            {
                TEntity entity = context.Set<TEntity>().Find(id);
                Delete<TEntity>(entity);
            });
            
        }

        public void Delete<TEntity>(TEntity entity)
            where TEntity : class
        {
            context.Set<TEntity>().Remove(entity);
        }

        public async void DeleteAsync<TEntity>(TEntity entity)
           where TEntity : class
        {
            await Task.Run(() =>
            {
                context.Set<TEntity>().Remove(entity);
            });           
        }

        public void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
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
    }
}

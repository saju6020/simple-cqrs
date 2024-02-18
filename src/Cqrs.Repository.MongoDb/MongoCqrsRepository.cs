namespace Platform.Infrastructure.Cqrs.Repository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using Platform.Infrastructure.Cqrs.Repository.Contracts;
    using Platform.Infrastructure.Cqrs.Repository.MongoDb;

    internal class MongoCqrsRepository : ICqrsRepository
    {
        private readonly DatabaseType databaseType;
        private readonly MongoDbConnectionCache mongoDbConnectionCache;

        public MongoCqrsRepository(DatabaseType databaseType, MongoDbConnectionCache mongoDbConnectionCache)
        {
            this.databaseType = databaseType;
            this.mongoDbConnectionCache = mongoDbConnectionCache;
        }

        public Task DeleteAsync<T>(Expression<Func<T, bool>> dataFilters)
        {
            return this.GetCollection<T>().DeleteOneAsync(dataFilters);
        }

        public Task<T> GetItemAsync<T>(Expression<Func<T, bool>> dataFilters)
        {
            return this.GetCollection<T>().Find(dataFilters).FirstOrDefaultAsync();
        }

        public IQueryable<T> GetItems<T>()
        {
            return this.GetCollection<T>().AsQueryable();
        }

        public IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters)
        {
            return this.GetCollection<T>().AsQueryable().Where(dataFilters);
        }

        public Task SaveAsync<T>(T data)
        {
            return this.GetCollection<T>().InsertOneAsync(data);
        }

        public Task UpdateAsync<T>(Expression<Func<T, bool>> dataFilters, T data)
        {
            return this.GetCollection<T>().ReplaceOneAsync(dataFilters, data);
        }

        public Task<bool> ExistAsync<T>(Expression<Func<T, bool>> dataFilters)
        {
            return this.GetCollection<T>().Find(dataFilters).AnyAsync();
        }

        public async Task<bool> ReplaceAsync<T>(Expression<Func<T, bool>> dataFilters, T data)
        {
            var replaceOneResult = await this.GetCollection<T>().ReplaceOneAsync(dataFilters, data);

            return replaceOneResult.ModifiedCount > 0;
        }

        private IMongoCollection<T> GetCollection<T>()
        {
            return this.mongoDbConnectionCache.GetVerticalDataContext(this.databaseType).GetCollection<T>($"{typeof(T).Name}s");
        }

        public Task UpsertAsync<T>(Expression<Func<T, bool>> dataFilters, T data)
        {
            return this.GetCollection<T>().ReplaceOneAsync(dataFilters, data, new ReplaceOptions { IsUpsert = true });
        }

        public Task SaveManyAsync<T>(IEnumerable<T> data)
        {
            return this.GetCollection<T>().InsertManyAsync(data);
        }

        public Task DeleteManyAsync<T>(Expression<Func<T, bool>> dataFilters)
        {
            return this.GetCollection<T>().DeleteManyAsync(dataFilters);
        }

    }
}

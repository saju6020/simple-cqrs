namespace Platform.Infrastructure.Repository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Platform.Infrastructure.Core;

    public class Repository : IMongoRepository
    {
        private readonly DatabaseType databaseType;
        private readonly MongoDbConnectionCache mongoDbConnectionCache;

        public Repository(MongoDbConnectionCache mongoDbConnectionCache)
        {
            this.databaseType = DatabaseType.Default;
            this.mongoDbConnectionCache = mongoDbConnectionCache;
        }

        public Repository(DatabaseType databaseType, MongoDbConnectionCache mongoDbConnectionCache)
        {
            this.databaseType = databaseType;
            this.mongoDbConnectionCache = mongoDbConnectionCache;
        }

        public Task DeleteAsync<T>(Expression<Func<T, bool>> dataFilters)
            where T : class
        {
            return this.GetCollection<T>().DeleteOneAsync(dataFilters);
        }

        public Task<T> GetItemAsync<T>(Expression<Func<T, bool>> dataFilters)
            where T:class
        {
            return this.GetCollection<T>().Find(dataFilters).FirstOrDefaultAsync();
        }

        public IQueryable<T> GetItems<T>()
            where T : class 
        {
            return this.GetCollection<T>().AsQueryable();
        }

        public IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters)
            where T : class
        {
            return this.GetCollection<T>().AsQueryable().Where(dataFilters);
        }

        public Task CreateAsync<T>(T data)
            where T : class
        {
            return this.GetCollection<T>().InsertOneAsync(data);
        }

        public Task UpdateAsync<T>(T data, Expression<Func<T, bool>> dataFilters)
            where T : class
        {
            return this.GetCollection<T>().ReplaceOneAsync(dataFilters, data);
        }

        public Task<bool> ExistAsync<T>(Expression<Func<T, bool>> dataFilters)
            where T : class
        {
            return this.GetCollection<T>().Find(dataFilters).AnyAsync();
        }

        public async Task<bool> ReplaceAsync<T>(T data, Expression<Func<T, bool>> dataFilters)
            where T : class
        {
            var replaceOneResult = await this.GetCollection<T>().ReplaceOneAsync(dataFilters, data);

            return replaceOneResult.ModifiedCount > 0;
        }

        private IMongoCollection<T> GetCollection<T>()
        {
            return this.mongoDbConnectionCache.GetVerticalDataContext(this.databaseType).GetCollection<T>($"{typeof(T).Name}s");
        }

        private IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return this.mongoDbConnectionCache.GetVerticalDataContext(this.databaseType).GetCollection<T>(collectionName);
        }

        public Task UpsertAsync<T>(T data,Expression<Func<T, bool>> dataFilters)
            where T : class
        {
            return this.GetCollection<T>().ReplaceOneAsync(dataFilters, data, new ReplaceOptions { IsUpsert = true });
        }

        public Task SaveManyAsync<T>(IEnumerable<T> data)
            where T : class
        {
            return this.GetCollection<T>().InsertManyAsync(data);
        }

        public Task DeleteManyAsync<T>(Expression<Func<T, bool>> dataFilters)
            where T : class
        {
            return this.GetCollection<T>().DeleteManyAsync(dataFilters);
        }

        public Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
            where T : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetItems<T>(BsonDocument[] pipeline, string collectionName)
        {
            var items = this.GetCollection<T>(collectionName)
                .Aggregate<T>(pipeline)
                .ToEnumerable();
            return Task.FromResult(items);
        }
    }
}

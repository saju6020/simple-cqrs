namespace Platform.Infrastructure.NoSql.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Platform.Infrastructure.NoSql.Repository.Contracts.Models;

    /// <summary>Mongo repository to perform operation with mongo db.</summary>
    /// <seealso cref="Platform.Infrastructure.MongoRepository.IMongoRepository" />
    public class MongoRepository : IMongoRepository
    {
        private const string CacheKeyPrefix = "MongoCS-";

        /// <summary>The dbconnection settings provider.</summary>
        private readonly IMongoDbConnectionSettingsProvider dbconnectionSettingsProvider;

        /// <summary>The mongo client.</summary>
        private IMongoClient mongoClient;

        /// <summary>The mongo database.</summary>
        private IMongoDatabase mongoDatabase;

        private IMemoryCache memoryCache;

        /// <summary>Initializes a new instance of the <see cref="MongoRepository" /> class.</summary>
        /// <param name="dbconnectionSettingsProvider">The dbconnection settings provider.</param>
        /// <param name="memoryCache">The memory cache.</param>
        public MongoRepository(IMongoDbConnectionSettingsProvider dbconnectionSettingsProvider, IMemoryCache memoryCache)
        {
            this.dbconnectionSettingsProvider = dbconnectionSettingsProvider;

            this.memoryCache = memoryCache;

            this.Initialize();
        }

        /// <summary>
        /// It return MongoDB Collection Object.
        /// </summary>
        /// <typeparam name="T">Db model.</typeparam>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>Mongo Collection.</returns>
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return this.mongoDatabase.GetCollection<T>(collectionName);
        }

        /// <summary>Gets the item.</summary>
        /// <typeparam name="T">User defined model.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <returns>Passed model as a template.</returns>
        public T GetItem<T>(Expression<Func<T, bool>> dataFilters)
        {
            return this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").AsQueryable().FirstOrDefault(dataFilters);
        }

        /// <summary>Gets the items.</summary>
        /// <typeparam name="T">Model want to pass.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <returns>User defined model.</returns>
        public IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters)
        {
            return this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").AsQueryable().Where(dataFilters);
        }

        /// <summary>Gets the items.</summary>
        /// <typeparam name="T">User defined model.</typeparam>
        /// <returns>Passed model to this method.</returns>
        public IQueryable<T> GetItems<T>()
        {
            return this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").AsQueryable();
        }

        /// <summary>Saves the specified data.</summary>
        /// <typeparam name="T">User defined model.</typeparam>
        /// <param name="data">The data.</param>
        public void Save<T>(T data)
        {
            this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").InsertOne(data);
        }

        /// <summary>Saves the specified data.</summary>
        /// <typeparam name="T">User defined model.</typeparam>
        /// <param name="data">The data.</param>
        public void Save<T>(List<T> data)
        {
            this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").InsertMany(data);
        }

        /// <summary>Deletes the specified data filters.</summary>
        /// <typeparam name="T">User defined model.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        public void Delete<T>(Expression<Func<T, bool>> dataFilters)
        {
            this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").DeleteMany(dataFilters);
        }

        /// <summary>Executes the command.</summary>
        /// <param name="command">The command.</param>
        /// <returns>Return command response.</returns>
        public ExecutedCommandResponse ExecuteCommand(string command)
        {
            var commandResponse = this.mongoDatabase.RunCommand(new BsonDocumentCommand<BsonDocument>(BsonDocument.Parse(command)));

            var response = new ExecutedCommandResponse();
            BsonValue bsonVlue;
            if (commandResponse.TryGetValue("ok", out bsonVlue))
            {
                var okvalue = bsonVlue.ToInt32();
                response.IsSuccess = okvalue == 1;
                return response;
            }

            response.Message = "Failed to execute RunCommand";
            return response;
        }

        /// <summary>Updates the specified data filters.</summary>
        /// <typeparam name="T">Used defined model.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <param name="data">The data.</param>
        public bool Update<T>(Expression<Func<T, bool>> dataFilters, T data)
        {
            ReplaceOneResult result = this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").ReplaceOne(dataFilters, data);

            return result.ModifiedCount > 0;
        }

        public bool UpdatePartial<T>(Expression<Func<T, bool>> dataFilters, T data)
        {
            var fieldValuePairs = this.GetValues(data);
            UpdateDefinition<T> update = this.GetUpdateDefination<T>(data);

            UpdateResult result = this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").UpdateOne(dataFilters, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> UpdatePartialAsync<T>(Expression<Func<T, bool>> dataFilters, object data)
        {
            var fieldValuePairs = this.GetValues(data);
            UpdateDefinition<T> update = this.GetUpdateDefination<T>(data);

            UpdateResult result = await this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").UpdateOneAsync(dataFilters, update).ConfigureAwait(false);
            return result.ModifiedCount > 0;
        }

        private UpdateDefinition<T> GetUpdateDefination<T>(object data)
        {
            var fieldValuePairs = this.GetValues(data);
            UpdateDefinition<T> update = null;

            foreach (var fieldValuePair in fieldValuePairs)
            {
                if (update == null)
                {
                    if (fieldValuePair.Value is string[])
                    {
                        update = Builders<T>.Update.Set(fieldValuePair.Key, (string[])fieldValuePair.Value);
                    }
                    else
                    {
                        update = Builders<T>.Update.Set(fieldValuePair.Key, fieldValuePair.Value);
                    }
                }
                else
                {
                    if (fieldValuePair.Value is string[])
                    {
                        update = update.Set(fieldValuePair.Key, (string[])fieldValuePair.Value);
                    }
                    else
                    {
                        update = update.Set(fieldValuePair.Key, fieldValuePair.Value);
                    }
                }
            }

            return update;
        }

        private IDictionary<string, object> GetValues(object obj)
        {
            return obj
                    .GetType()
                    .GetProperties()
                    .ToDictionary(p => p.Name, p => p.GetValue(obj) == null ? null : p.GetValue(obj));
        }

        private void CacheMongoClient(MongoClient mongoClient, string connectionString)
        {
            this.memoryCache.Set(CacheKeyPrefix + connectionString, mongoClient);
        }

        public MongoClient GetMongoClient(string connectionString)
        {
            var found = this.memoryCache.TryGetValue(CacheKeyPrefix + connectionString, out MongoClient mongoClient);

            if (found)
            {
                return mongoClient;
            }
            else
            {
                MongoClient newMongoClient = new MongoClient(connectionString);
                this.CacheMongoClient(newMongoClient, connectionString);
                return newMongoClient;
            }
        }

        /// <summary>Initializes this instance.</summary>
        private void Initialize()
        {
            var dbconnectionSettings = this.dbconnectionSettingsProvider.GetConnectionSettings();
            this.mongoClient = this.GetMongoClient(dbconnectionSettings.ConnectionString);
            this.mongoDatabase = this.mongoClient.GetDatabase(dbconnectionSettings.DatabaseName);
        }

        /// <summary>Saves the asynchronous.</summary>
        /// <typeparam name="T">Model.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns><Type>A <see cref="Task"/> representing the asynchronous operation.</Type></returns>
        public async Task SaveAsync<T>(T data)
        {
            await this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").InsertOneAsync(data).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <typeparam name="T">Model.</typeparam>
        /// <param name="session">The session.</param>
        /// <param name="data">The data.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SaveAsync<T>(IClientSessionHandle session, T data, string collectionName = null)
        {
            string collection = collectionName ?? $"{typeof(T).Name}s";
            await this.mongoDatabase.GetCollection<T>(collection).InsertOneAsync(session, data).ConfigureAwait(false);
        }

        /// <summary>
        /// It reurns a List of MongDB Entity that matches filtering.
        /// It supports GeoSpacial query too.
        /// </summary>
        /// <typeparam name="T">MongoDB Entity.</typeparam>
        /// <param name="filterDefinition">Mongo Filter Definition.</param>
        /// <param name="collectionName">Exact name of the collection. Usually Collection name slightly differ from the Entity name.</param>
        /// <returns>A List of expected Entity that matched provided filter.</returns>
        public Task<IEnumerable<T>> GetItems<T>(FilterDefinition<T> filterDefinition, string collectionName)
        {
            var items = this.mongoDatabase.GetCollection<T>(collectionName)
                .Find(filterDefinition)
                .ToEnumerable();
            return Task.FromResult(items);
        }

        /// <summary>Gets the items asynchronous.</summary>
        /// <typeparam name="T">Expression.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <returns>Qurable task.</returns>
        public async Task<IQueryable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> dataFilters)
        {
            return await Task.Run(() =>
            {
                return this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").AsQueryable().Where(dataFilters);
            }).ConfigureAwait(false);
        }

        /// <summary>Gets the items asynchronous.</summary>
        /// <typeparam name="T">Poco object.</typeparam>
        /// <returns>Qurable task.</returns>
        public async Task<IQueryable<T>> GetItemsAsync<T>()
        {
            return await Task.Run(() =>
            {
                return this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").AsQueryable();
            }).ConfigureAwait(false);
        }

        /// <summary>Saves the asynchronous.</summary>
        /// <typeparam name="T">List of poco object.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns><Task>A <see cref="Task"/> representing the asynchronous operation.</Task></returns>
        public async Task SaveAsync<T>(List<T> data)
        {
            await this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").InsertManyAsync(data).ConfigureAwait(false);
        }

        /// <summary>Gets the item asynchronous.</summary>
        /// <typeparam name="T">Expressiin.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <returns>Task.</returns>
        public async Task<T> GetItemAsync<T>(Expression<Func<T, bool>> dataFilters)
        {
            return await this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s")
                .Find(dataFilters).SingleOrDefaultAsync().ConfigureAwait(false);
        }

        /// <summary>Deletes the asynchronous.</summary>
        /// <typeparam name="T">Expression.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DeleteAsync<T>(Expression<Func<T, bool>> dataFilters)
        {
            await this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").DeleteManyAsync(dataFilters).ConfigureAwait(false);
        }

        /// <summary>Updates the asynchronous.</summary>
        /// <typeparam name="T">Expression.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <param name="data">The data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<bool> UpdateAsync<T>(Expression<Func<T, bool>> dataFilters, T data)
        {
            ReplaceOneResult result = await this.mongoDatabase.GetCollection<T>($"{typeof(T).Name}s").ReplaceOneAsync(dataFilters, data).ConfigureAwait(false);

            return result.ModifiedCount > 0;
        }

        /// <summary>Executes the command asynchronous.</summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task<ExecutedCommandResponse> ExecuteCommandAsync(string command)
        {
            var commandResponse = await this.mongoDatabase.RunCommandAsync(
                new BsonDocumentCommand<BsonDocument>(BsonDocument.Parse(command))).ConfigureAwait(false);

            var response = new ExecutedCommandResponse();
            BsonValue bsonVlue;
            if (commandResponse.TryGetValue("ok", out bsonVlue))
            {
                var okvalue = bsonVlue.ToInt32();
                response.IsSuccess = okvalue == 1;
                return response;
            }

            response.Message = "Failed to execute RunCommand";
            return response;
        }

        /// <summary>Gets the items.</summary>
        /// <typeparam name="T">Db Model.</typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>List.</returns>
        public Task<IEnumerable<T>> GetItems<T>(BsonDocument[] pipeline, string collectionName)
        {
            var items = this.mongoDatabase.GetCollection<T>(collectionName)
                .Aggregate<T>(pipeline)
                .ToEnumerable();
            return Task.FromResult(items);
        }

        public void Save<T>(T data, string collectionName)
        {
            this.mongoDatabase.GetCollection<T>(collectionName).InsertOne(data);
        }

        public void Save<T>(List<T> data, string collectionName)
        {
            this.mongoDatabase.GetCollection<T>(collectionName).InsertMany(data);
        }

        public async Task SaveAsync<T>(T data, string collectionName)
        {
            await this.mongoDatabase.GetCollection<T>(collectionName).InsertOneAsync(data).ConfigureAwait(false);
        }

        public async Task SaveAsync<T>(List<T> data, string collectionName)
        {
            await this.mongoDatabase.GetCollection<T>(collectionName).InsertManyAsync(data).ConfigureAwait(false);
        }

        public void Update<T>(Expression<Func<T, bool>> dataFilters, T data, string collectionName)
        {
            this.mongoDatabase.GetCollection<T>(collectionName).ReplaceOne(dataFilters, data);
        }

        public async Task UpdateAsync<T>(Expression<Func<T, bool>> dataFilters, T data, string collectionName)
        {
            await this.mongoDatabase.GetCollection<T>(collectionName).ReplaceOneAsync(dataFilters, data).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <typeparam name="T">Model.</typeparam>
        /// <param name="session">The session.</param>
        /// <param name="dataFilters">The data filters.</param>
        /// <param name="data">The data.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UpdateAsync<T>(IClientSessionHandle session, Expression<Func<T, bool>> dataFilters, T data, string collectionName = null)
        {
            string collection = collectionName ?? $"{typeof(T).Name}s";
            await this.mongoDatabase.GetCollection<T>(collection).ReplaceOneAsync(session, dataFilters, data).ConfigureAwait(false);
        }

        public void Delete<T>(Expression<Func<T, bool>> dataFilters, string collectionName)
        {
            this.mongoDatabase.GetCollection<T>(collectionName).DeleteMany(dataFilters);
        }

        public async Task DeleteAsync<T>(Expression<Func<T, bool>> dataFilters, string collectionName)
        {
            await this.mongoDatabase.GetCollection<T>(collectionName).DeleteManyAsync(dataFilters).ConfigureAwait(false);
        }

        public IClientSessionHandle StartSession()
        {
            return this.mongoClient.StartSession();
        }

        public async Task UpdateManyAsync<T>(Expression<Func<T, bool>> dataFilters, object data, string collectionName = "")
        {
            var fieldValuePairs = this.GetValues(data);
            UpdateDefinition<T> update = null;

            foreach (var fieldValuePair in fieldValuePairs)
            {
                if (update == null)
                {
                    if (fieldValuePair.Value is string[])
                    {
                        update = Builders<T>.Update.Set(fieldValuePair.Key, (string[])fieldValuePair.Value);
                    }
                    else
                    {
                        update = Builders<T>.Update.Set(fieldValuePair.Key, fieldValuePair.Value);
                    }
                }
                else
                {
                    if (fieldValuePair.Value is string[])
                    {
                        update.AddToSet(fieldValuePair.Key, (string[])fieldValuePair.Value);
                    }
                    else
                    {
                        update.AddToSet(fieldValuePair.Key, fieldValuePair.Value);
                    }
                }
            }

            if (string.IsNullOrEmpty(collectionName))
            {
                collectionName = $"{typeof(T).Name}s";
            }

            await this.mongoDatabase.GetCollection<T>(collectionName).UpdateManyAsync(dataFilters, update).ConfigureAwait(false);
        }
    }
}

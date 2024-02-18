namespace Platform.Infrastructure.Cqrs.Repository.MongoDb
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using MongoDB.Driver;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.ServiceRegistry;

    public class MongoDbConnectionCache
    {
        private const string DatabaseIdFormat = "{0}-{1}-{2}-{3}";

        private readonly string serviceId;

        private readonly IUserContextProvider userContextProvider;

        private readonly IDictionary<string, IMongoDatabase> cachedMongoDatabases = new SortedDictionary<string, IMongoDatabase>();

        public MongoDbConnectionCache(IConfiguration configuration, IServiceRegistryProvider serviceRegistryProvider, IUserContextProvider userContextProvider)
        {
            this.serviceId = configuration["ServiceId"];

            foreach (var vertical in serviceRegistryProvider.GetAllServices().SelectMany(service => service.Verticals))
            {
                // Read database
                if (!string.IsNullOrWhiteSpace(vertical.ReadServerConnectionString) || !string.IsNullOrWhiteSpace(vertical.ReadDatabaseName))
                {
                    var readDatabase = GetMongoDatabase(vertical.ReadServerConnectionString, vertical.ReadDatabaseName);

                    var readDatabaseId = string.Format(DatabaseIdFormat, vertical.ServiceId, vertical.TenantId, vertical.Id, DatabaseType.Read);

                    this.cachedMongoDatabases.Add(readDatabaseId, readDatabase);
                }

                // State database
                if (!string.IsNullOrWhiteSpace(vertical.StateServerConnectionString) || !string.IsNullOrWhiteSpace(vertical.StateDatabaseName))
                {
                    var stateDatabase = GetMongoDatabase(vertical.StateServerConnectionString, vertical.StateDatabaseName);

                    var stateDatabaseId = string.Format(DatabaseIdFormat, vertical.ServiceId, vertical.TenantId, vertical.Id, DatabaseType.State);

                    this.cachedMongoDatabases.Add(stateDatabaseId, stateDatabase);
                }

                // Event database
                if (!string.IsNullOrWhiteSpace(vertical.EventServerConnectionString) || !string.IsNullOrWhiteSpace(vertical.EventDatabaseName))
                {
                    var eventDatabase = GetMongoDatabase(vertical.EventServerConnectionString, vertical.EventDatabaseName);

                    var eventDatabaseId = string.Format(DatabaseIdFormat, vertical.ServiceId, vertical.TenantId, vertical.Id, DatabaseType.Event);

                    this.cachedMongoDatabases.Add(eventDatabaseId, eventDatabase);
                }
            }

            this.userContextProvider = userContextProvider;
        }

        public IMongoDatabase GetVerticalDataContext(DatabaseType databaseType)
        {
            return this.GetVerticalDataContext(databaseType, this.userContextProvider.GetUserContext());
        }

        public IMongoDatabase GetVerticalDataContext(DatabaseType databaseType, UserContext userContext)
        {
            var databaseId = string.Format(DatabaseIdFormat, serviceId ?? userContext.ServiceId, userContext.TenantId, userContext.VerticalId, databaseType);

            return this.cachedMongoDatabases[databaseId];
        }

        private static IMongoDatabase GetMongoDatabase(string databaseConnectionString, string databaseName)
        {
            var mongoUrl = new MongoUrl(databaseConnectionString);
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoUrl);
            mongoClientSettings.RetryWrites = true;
            return new MongoClient(mongoClientSettings).GetDatabase(databaseName);
        }
    }
}

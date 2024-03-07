namespace Platform.Infrastructure.ServiceRegistry
{
    using System;

    public class Vertical
    {
        public Vertical(
            Guid id,
            string name,
            App[] apps,
            Feature[] features,
            string readDatabaseName,
            string stateDatabaseName,
            string eventDatabaseName,
            string storageBucketName,
            string cacheDatabaseName,
            string readServerConnectionString,
            string stateServerConnectionString,
            string eventServerConnectionString,
            string storageServerConnectionString,
            string cacheServerConnectionString)
        {
            this.Id = id;
            this.Name = name;
            this.Apps = apps;
            this.Features = features;
            this.ReadDatabaseName = readDatabaseName;
            this.StateDatabaseName = stateDatabaseName;
            this.EventDatabaseName = eventDatabaseName;
            this.StorageBucketName = storageBucketName;
            this.CacheDatabaseName = cacheDatabaseName;
            this.ReadServerConnectionString = readServerConnectionString;
            this.StateServerConnectionString = stateServerConnectionString;
            this.EventServerConnectionString = eventServerConnectionString;
            this.StorageServerConnectionString = storageServerConnectionString;
            this.CacheServerConnectionString = cacheServerConnectionString;
        }

        public Guid Id { get; }

        public string ServiceId { get; private set; }

        public Guid TenantId { get; private set; }

        public string Name { get; }

        public App[] Apps { get; }

        public Feature[] Features { get; }

        public string ReadDatabaseName { get; }

        public string StateDatabaseName { get; }

        public string EventDatabaseName { get; }

        public string StorageBucketName { get; }

        public string CacheDatabaseName { get; }

        public string ReadServerConnectionString { get; }

        public string StateServerConnectionString { get; }

        public string EventServerConnectionString { get; }

        public string StorageServerConnectionString { get; }

        public string CacheServerConnectionString { get; }

        public void SetServiceId(string serviceId)
        {
            this.ServiceId = serviceId;
        }

        public void SetTenantId(Guid tenantId)
        {
            this.TenantId = tenantId;
        }
    }
}
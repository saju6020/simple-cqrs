namespace Platform.Infrastructure.NoSql.Repository
{
    using System;
    using Platform.Infrastructure.AppConfigurationManager;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.NoSql.Repository.Contracts.Models;

    /// <summary>Mongo db connection setting provider.</summary>
    /// <seealso cref="Shohoz.Platform.Infrastructure.MongoRepository.IMongoDbConnectionSettingsProvider" />
    public class MongoDbConnectionSettingProvider : IMongoDbConnectionSettingsProvider
    {
        /// <summary>The application configuration manager.</summary>
        private readonly IAppConfigurationManager appConfigurationManager;

        /// <summary>The security claim manager.</summary>
        private readonly IUserContextProvider userContextProvider;

        /// <summary>Initializes a new instance of the <see cref="MongoDbConnectionSettingProvider" /> class.</summary>
        /// <param name="appConfigurationManager">The application configuration manager.</param>
        /// <param name="userContextProvider">The user context provider.</param>
        public MongoDbConnectionSettingProvider(IAppConfigurationManager appConfigurationManager, IUserContextProvider userContextProvider)
        {
            this.appConfigurationManager = appConfigurationManager;
            this.userContextProvider = userContextProvider;
        }

        /// <summary>Gets the connection settings.</summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>Connection settings.</returns>
        public DbConnectionSettings GetConnectionSettings(Guid tenantId)
        {
            if (tenantId.Equals(Guid.Empty))
            {
                return null;
            }

            DbConnectionSettings dbconnectionSettings = new DbConnectionSettings();

            UserContext userContext = this.userContextProvider.GetUserContext();
            GlobalSettings globalSettings = this.appConfigurationManager.GlobalSettings;

            string mongoConnectionURI = globalSettings.MongoDBConnectionURI;

            int mongoConnectionURILength = mongoConnectionURI.Length;

            if (mongoConnectionURI.EndsWith("/"))
            {
                mongoConnectionURI = mongoConnectionURI.Remove(mongoConnectionURILength - 1);
            }

            dbconnectionSettings.ConnectionString = $"{mongoConnectionURI}/{tenantId}";
            dbconnectionSettings.TenantId = tenantId;
            dbconnectionSettings.DatabaseName = tenantId.ToString();
            dbconnectionSettings.VerticalId = userContext.VerticalId.ToString();

            return dbconnectionSettings;
        }

        /// <summary>Gets the connection settings.</summary>
        /// <returns>Connection settings.</returns>
        public DbConnectionSettings GetConnectionSettings()
        {
            IAppSettings appsettings = this.appConfigurationManager.AppSettings;
            string databaseConnectionString = appsettings.DatabaseConnectionString;
            string databaseName = appsettings.DatabaseName;
            UserContext userContext = this.userContextProvider.GetUserContext();

            DbConnectionSettings dbconnectionSettings = new DbConnectionSettings();

            databaseConnectionString = databaseConnectionString?.Trim();

            dbconnectionSettings.TenantId = userContext.TenantId;
            dbconnectionSettings.VerticalId = userContext.VerticalId.ToString();

            if (!string.IsNullOrEmpty(databaseConnectionString))
            {
                dbconnectionSettings.ConnectionString = databaseConnectionString;
            }

            if (!string.IsNullOrEmpty(databaseName))
            {
                dbconnectionSettings.DatabaseName = databaseName;
            }

            if (string.IsNullOrEmpty(dbconnectionSettings.ConnectionString) && string.IsNullOrEmpty(dbconnectionSettings.DatabaseName))
            {
                dbconnectionSettings = this.GetConnectionSettings(userContext.TenantId);
            }

            return dbconnectionSettings;
        }
    }
}

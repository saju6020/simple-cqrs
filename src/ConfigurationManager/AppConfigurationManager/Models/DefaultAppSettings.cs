namespace Platform.Infrastructure.AppConfigurationManager
{
    /// <summary>Default App Settings Model.</summary>
    /// <seealso cref="Shohoz.Core.Infrastructure.AppConfigurationManager.IAppSettings" />
    public class DefaultAppSettings : IAppSettings
    {
        /// <summary>Gets or sets the name of the service.</summary>
        /// <value>The name of the service.</value>
        public string ServiceName { get; set; }

        /// <summary>Gets or sets the database connection string.</summary>
        /// <value>The database connection string.</value>
        public string DatabaseConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        public string DatabaseName { get; set; }
    }
}

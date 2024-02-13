namespace Platform.Infrastructure.AppConfigurationManager
{
    /// <summary>App settings abstraction.</summary>
    public interface IAppSettings
    {
        /// <summary>Gets or sets the name of the service.</summary>
        /// <value>The name of the service.</value>
        string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        string DatabaseName { get; set; }

        /// <summary>Gets or sets the database connection string.</summary>
        /// <value>The database connection string.</value>
        string DatabaseConnectionString { get; set; }
    }
}
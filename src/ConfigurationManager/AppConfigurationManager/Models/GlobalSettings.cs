namespace Platform.Infrastructure.AppConfigurationManager
{
    /// <summary>Global Settings Model.</summary>
    public class GlobalSettings
    {
        /// <summary>Gets or sets the name of the service.</summary>
        /// <value>The name of the service.</value>
        public string ServiceName { get; set; }

        /// <summary>Gets or sets the API host URI.</summary>
        /// <value>The API host URI.</value>
        public string ApiHostUri { get; set; }

        /// <summary>Gets or sets the scheme.</summary>
        /// <value>The scheme.</value>
        public string Scheme { get; set; }

        /// <summary>Gets or sets the default lay out.</summary>
        /// <value>The default lay out.</value>
        public string DefaultLayOut { get; set; }

        /// <summary>Gets or sets the default log level.</summary>
        /// <value>The default log level.</value>
        public string DefaultLogLevel { get; set; }

        /// <summary>Gets or sets the default file path.</summary>
        /// <value>The default file path.</value>
        public string DefaultFilePath { get; set; }

        /// <summary>Gets or sets the application insights instrumentation key.</summary>
        /// <value>The application insights instrumentation key.</value>
        public string ApplicationInsightsInstrumentationKey { get; set; }

        /// <summary>Gets or sets the azure connection string.</summary>
        /// <value>The azure connection string.</value>
        public string AzureGlobalAppConfigConnectionString { get; set; }

        /// <summary>Gets or sets the mongo database connection URI.</summary>
        /// <value>The mongo database connection URI.</value>
        public string MongoDBConnectionURI { get; set; }

        /// <summary>Gets or sets the azure connection string.</summary>
        /// <value>The azure connection string.</value>
        public string AzureConnectionString { get; set; }

        /// <summary>
        /// Gets or sets AWS access key.
        /// </summary>
        public string AwsAccessKey { get; set; }

        /// <summary>
        /// Gets or sets AWS secret key.
        /// </summary>
        public string AwsSecretKey { get; set; }

        /// <summary>
        /// Gets or sets AWS region.
        /// </summary>
        public string AwsRegion { get; set; }

        /// <summary>
        /// Gets or sets SeqServerUrl.
        /// </summary>
        public string SeqServerUrl { get; set; }

        public string BlobContainerName { get; set; }
    }
}

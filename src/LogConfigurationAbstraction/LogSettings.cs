namespace Platform.Infrastructure.LogConfiguration.Abstraction
{
    /// <summary>Log settings model.</summary>
    public class LogSettings
    {
        /// <summary>Gets or sets the default lay out.</summary>
        /// <value>The default lay out.</value>
        public string DefaultLayOut { get; set; }

        /// <summary>Gets or sets the default log level.</summary>
        /// <value>The default log level.</value>
        public string DefaultLogLevel { get; set; }

        /// <summary>Gets or sets the name of the service.</summary>
        /// <value>The name of the service.</value>
        public string ServiceName { get; set; }

        /// <summary>Gets or sets the default file path.</summary>
        /// <value>The default file path.</value>
        public string DefaultFilePath { get; set; }

        /// <summary>Gets or sets the default seq address.</summary>
        /// <value>The default seq address.</value>
        public string DefaultSeqAddress { get; set; } = "http://localhost:5341";

        /// <summary>Gets or sets the application insights instrumentation key.</summary>
        /// <value>The application insights instrumentation key.</value>
        public string ApplicationInsightsInstrumentationKey { get; set; }

        /// <summary>Gets or sets the azure connection string.</summary>
        /// <value>The azure connection string.</value>
        public string AzureConnectionString { get; set; }

        /// <summary>
        /// Gets or sets AwsAccessKey.
        /// </summary>
        public string AwsAccessKey { get; set; }

        /// <summary>
        /// Gets or sets AwsSecretKey.
        /// </summary>
        public string AwsSecretKey { get; set; }

        /// <summary>
        /// Gets or sets AwsRegion.
        /// </summary>
        public string AwsRegion { get; set; }

        /// <summary>
        /// Gets or sets SeqServerUrl.
        /// </summary>
        public string SeqServerUrl { get; set; }

        public string BlobContainerName { get; set; }
    }
}

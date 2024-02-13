namespace Platform.Infrastructure.LogConfiguration.Abstraction
{
    using Platform.Infrastructure.AppConfigurationManager;

    /// <summary>This class is to initialize log settings.</summary>
    public class LogSettingsInitializer
    {
        /// <summary>The application configuration manager.</summary>
        private readonly IAppConfigurationManager appConfigurationManager;

        /// <summary>Initializes a new instance of the <see cref="LogSettingsInitializer"/> class.</summary>
        /// <param name="configurationMgr">Param configuration manager.</param>
        public LogSettingsInitializer(IAppConfigurationManager configurationMgr)
        {
            this.appConfigurationManager = configurationMgr;
        }

        /// <summary>Gets or sets the log settings.</summary>
        /// <value>The log settings.</value>
        private LogSettings LogSettings { get; set; }

        /// <summary>Initializes this instance.</summary>
        /// <returns>Log settings.</returns>
        public LogSettings Initialize()
        {
            this.LogSettings = new LogSettings();
            this.SetServiceNameAndDefaultLogLevel();
            this.SetDefaultLogFilePath();
            this.SetApplicationInsightsInstrumentationKey();
            this.SetAzureConnectionString();
            this.SetDefaultOutPutLayOut();
            this.SetAwsCloudWatchSettings();
            this.SetSeqServerUrl();
            return this.LogSettings;
        }

        private void SetSeqServerUrl()
        {
            this.LogSettings.SeqServerUrl = this.appConfigurationManager.GlobalSettings.SeqServerUrl;
        }

        /// <summary>
        /// Sets Aws settings.
        /// </summary>
        private void SetAwsCloudWatchSettings()
        {
            this.LogSettings.AwsAccessKey = this.appConfigurationManager.GlobalSettings.AwsAccessKey;
            this.LogSettings.AwsSecretKey = this.appConfigurationManager.GlobalSettings.AwsSecretKey;
            this.LogSettings.AwsRegion = this.appConfigurationManager.GlobalSettings.AwsRegion;
        }

        /// <summary>Sets the default out put lay out.</summary>
        private void SetDefaultOutPutLayOut()
        {
            this.LogSettings.DefaultLayOut = this.appConfigurationManager.GlobalSettings.DefaultLayOut;
        }

        /// <summary>Sets the service name and default log level.</summary>
        private void SetServiceNameAndDefaultLogLevel()
        {
            this.LogSettings.ServiceName = this.appConfigurationManager.AppSettings.ServiceName;
            this.LogSettings.DefaultLogLevel = this.appConfigurationManager.GlobalSettings.DefaultLogLevel;
        }

        /// <summary>Sets the azure connection string.</summary>
        private void SetAzureConnectionString()
        {
            this.LogSettings.AzureConnectionString = this.appConfigurationManager.GlobalSettings.AzureConnectionString;
            this.LogSettings.BlobContainerName = this.appConfigurationManager.GlobalSettings.BlobContainerName;
        }

        /// <summary>Sets the default log file path.</summary>
        private void SetDefaultLogFilePath()
        {
            this.LogSettings.DefaultFilePath = this.appConfigurationManager.GlobalSettings.DefaultFilePath + $"//{this.LogSettings.ServiceName}//";
        }

        /// <summary>Sets the application insights instrumentation key.</summary>
        private void SetApplicationInsightsInstrumentationKey()
        {
            this.LogSettings.ApplicationInsightsInstrumentationKey = this.appConfigurationManager.GlobalSettings.ApplicationInsightsInstrumentationKey;
        }
    }
}
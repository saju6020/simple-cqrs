namespace Platform.Infrastructure.LogConfiguration.SerilogConfiguration
{
    using System;
    using Serilog;
    using Serilog.Sinks.AwsCloudWatch;
    using Platform.Infrastructure.LogConfiguration.Abstraction;

    /// <summary>Configure Serilog.</summary>
    /// <seealso cref="Core.Infrastructure.LogConfiguration.Common.ILogConfiguration" />
    public class SeriLogConfiguration : ILogConfiguration
    {
        /// <summary>Initializes a new instance of the <see cref="SeriLogConfiguration"/> class.</summary>
        public SeriLogConfiguration()
        {
            this.LoggerConfiguration = new LoggerConfiguration();
        }

        /// <summary>Gets or sets the log settings.</summary>
        /// <value>The log settings.</value>
        public LogSettings LogSettings { get; set; }

        /// <summary>Gets the logger configuration.</summary>
        /// <value>The logger configuration.</value>
        private LoggerConfiguration LoggerConfiguration { get; }

        /// <summary>Add WriteToConsole configuration in LoggerConfiguration.</summary>
        /// <param name="layOut">The lay out.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="retainedFileCountLimit">The retained file count limit.</param>
        /// <returns>It returns current ILogConfiguration.</returns>
        public ILogConfiguration UseConsole(string layOut = "", LogLevel logLevel = LogLevel.INFO, int retainedFileCountLimit = 10)
        {
            layOut = this.GetLayout(layOut);
            this.LoggerConfiguration.WriteTo.Async(a => a.Console(outputTemplate: layOut, restrictedToMinimumLevel: SeriLogLevelProvider.GetLogLevel(logLevel)));
            return this;
        }

        /// <summary>Add WriteToFile configuration in LoggerConfiguration.</summary>
        /// <param name="path">The path.</param>
        /// <param name="layOut">The lay out.</param>
        /// <param name="fileSizeLimitInBytes">The file size limit in byte.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="retainedFileCountLimit">The retained file count limit.</param>
        /// <param name="buffered">should use buffer or not.</param>
        /// <returns>It returns current ILogConfiguration.</returns>
        public ILogConfiguration UseFile(
            string path = "",
            string layOut = "",
            long fileSizeLimitInBytes = 1000000,
            LogLevel logLevel = LogLevel.INFO,
            int retainedFileCountLimit = 10,
            bool buffered = true)
        {
            path = this.GetFilePath(path);
            layOut = this.GetLayout(layOut);
            logLevel = !string.IsNullOrEmpty(this.LogSettings.DefaultLogLevel)
                ? InternalLogLevelProvider.GetInternalLogLevel(this.LogSettings.DefaultLogLevel)
                : logLevel;

            this.LoggerConfiguration.WriteTo.Async(a => a.File(
                path,
                SeriLogLevelProvider.GetLogLevel(logLevel),
                outputTemplate: layOut,
                null,
                retainedFileCountLimit: retainedFileCountLimit,
                buffered: buffered,
                fileSizeLimitBytes: fileSizeLimitInBytes,
                rollingInterval: RollingInterval.Day,
                flushToDiskInterval: TimeSpan.FromMilliseconds(200)));
            return this;
        }

        /// <summary>Add WriteToAzureBlob configuration in LoggerConfiguration.</summary>
        /// <param name="layOut">The lay out.</param>
        /// <param name="logLevel">The log level.</param>
        /// <returns>It returns current ILogConfiguration.</returns>
        public ILogConfiguration UseAzureBlob(string layOut = "", LogLevel logLevel = LogLevel.INFO)
        {
            layOut = this.GetLayout(layOut);
            logLevel = !string.IsNullOrEmpty(this.LogSettings.DefaultLogLevel)
                ? InternalLogLevelProvider.GetInternalLogLevel(this.LogSettings.DefaultLogLevel)
                : logLevel;

            this.LoggerConfiguration.WriteTo.Async(a => a.AzureBlobStorage(
                connectionString: this.LogSettings.AzureConnectionString,
                restrictedToMinimumLevel: SeriLogLevelProvider.GetLogLevel(logLevel),
                storageContainerName: this.LogSettings.BlobContainerName,
                storageFileName: $"{LogSettings.ServiceName}/{DateTime.UtcNow.Year}-{DateTime.UtcNow.Month}-{DateTime.UtcNow.Day}.txt"));

            return this;
        }

        /// <summary>Uses the application in sight.</summary>
        /// <param name="logLevel">The log level.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public ILogConfiguration UseAppInSight(LogLevel logLevel = LogLevel.INFO)
        {
            logLevel = !string.IsNullOrEmpty(this.LogSettings.DefaultLogLevel)
                ? InternalLogLevelProvider.GetInternalLogLevel(this.LogSettings.DefaultLogLevel)
                : logLevel;

           // this.LoggerConfiguration.WriteTo.Async(a => a.ApplicationInsightsEvents(this.LogSettings.ApplicationInsightsInstrumentationKey, restrictedToMinimumLevel: SeriLogLevelProvider.GetLogLevel(logLevel)));
            return this;
        }

        /// <summary>Users the aws cloud.</summary>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="queueSize">Size of the queue.</param>
        /// <param name="logLevel">The log level.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public ILogConfiguration UserAwsCloud(
            int batchSize = 100,
            int queueSize = 10000,
            LogLevel logLevel = LogLevel.INFO)
        {
            string logGroupName = !string.IsNullOrEmpty(this.LogSettings.ServiceName) ? this.LogSettings.ServiceName : "DefaultLogGroup";
            logLevel = !string.IsNullOrEmpty(this.LogSettings.DefaultLogLevel)
                ? InternalLogLevelProvider.GetInternalLogLevel(this.LogSettings.DefaultLogLevel)
                : logLevel;
            AwsHelper.RegisterProfile(this.LogSettings);
            var options = AwsHelper.GetCloudWatchSinkOptions(logGroupName, batchSize, queueSize, logLevel);
            var client = AwsHelper.GetAwsCloudWatchClient(this.LogSettings.AwsRegion);

            this.LoggerConfiguration.WriteTo.Async(a => a.AmazonCloudWatch(options, client));

            return this;
        }

        public ILogConfiguration UseSeq(LogLevel logLevel = LogLevel.INFO)
        {
            logLevel = !string.IsNullOrEmpty(this.LogSettings.DefaultLogLevel)
                ? InternalLogLevelProvider.GetInternalLogLevel(this.LogSettings.DefaultLogLevel)
                : logLevel;

            this.LoggerConfiguration.WriteTo.Async(a => a.Seq(
                serverUrl: this.LogSettings.SeqServerUrl,
                restrictedToMinimumLevel: SeriLogLevelProvider.GetLogLevel(logLevel)));

            return this;
        }

        /// <summary>Creates Logger from the LogConfiguration.</summary>
        /// <returns>It returns current ILogConfiguration.</returns>
        public ILogConfiguration Initiate()
        {
            this.LoggerConfiguration.MinimumLevel.Debug();
            this.LoggerConfiguration.Enrich.WithCorrelationId();
            Log.Logger = this.LoggerConfiguration.CreateLogger();
            return this;
        }

        /// <summary>
        /// It checks if any layout is provided otherwise returns default layout from log setting.
        /// </summary>
        /// <param name="layOut">layout patterns passed to the method. </param>
        /// <returns> Return log layout.</returns>
        private string GetLayout(string layOut)
        {
            layOut = string.IsNullOrEmpty(layOut) ? this.LogSettings.DefaultLayOut : layOut;
            return layOut;
        }

        /// <summary>
        ///  It checks if any file path is provided otherwise returns default file path from log setting.
        /// </summary>
        /// <param name="path"> path passed to the method.</param>
        /// <returns> file path.</returns>
        private string GetFilePath(string path)
        {
            path = string.IsNullOrEmpty(path) ? this.LogSettings.DefaultFilePath : path;
            return path;
        }
    }
}

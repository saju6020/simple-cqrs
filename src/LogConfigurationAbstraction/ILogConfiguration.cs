namespace Platform.Infrastructure.LogConfiguration.Abstraction
{
    /// <summary>
    /// ILogConfiguration includes common signatures that should be implemented while configuring any logging framework.
    /// </summary>
    public interface ILogConfiguration
    {
        /// <summary>
        /// Add WriteToFile configuration in LoggerConfiguration.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="layOut">The lay out.</param>
        /// <param name="fileSizeLimitInBytes">The file size limit in byte.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="retainedFileCountLimit">The retained file count limit.</param>
        /// <param name="buffered">Use Buffer or not.</param>
        /// <returns> It returns current ILogConfiguration. </returns>
        ILogConfiguration UseFile(string path = "", string layOut = "", long fileSizeLimitInBytes = 1000000, LogLevel logLevel = LogLevel.INFO, int retainedFileCountLimit = 10, bool buffered = true);

        /// <summary>
        /// Add WriteToConsole configuration in LoggerConfiguration.
        /// </summary>
        /// <param name="layOut">The lay out.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="retainedFileCountLimit">The retained file count limit.</param>
        /// <returns> It returns current ILogConfiguration. </returns>
        ILogConfiguration UseConsole(string layOut = "", LogLevel logLevel = LogLevel.INFO, int retainedFileCountLimit = 10);

        /// <summary>
        /// Add WriteToAzureBlob configuration in LoggerConfiguration.
        /// </summary>
        /// <param name="layOut">The lay out.</param>
        /// <param name="logLevel">The log level.</param>
        /// <returns> It returns current ILogConfiguration. </returns>
        ILogConfiguration UseAzureBlob(string layOut = "", LogLevel logLevel = LogLevel.INFO);

        /// <summary>Uses the application in sight.</summary>
        /// <param name="logLevel">The log level.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        ILogConfiguration UseAppInSight(LogLevel logLevel = LogLevel.INFO);

        /// <summary>Users the aws cloud.</summary>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="queueSize">Size of the queue.</param>
        /// <param name="minimumLogLevel">The minimum log level.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        ILogConfiguration UserAwsCloud(
            int batchSize = 100,
            int queueSize = 10000,
            LogLevel minimumLogLevel = LogLevel.INFO);

        /// <summary>
        /// Add Seq in LoggerConfiguration.
        /// </summary>
        /// <param name="logLevel">LogLevel.</param>
        /// <returns>It returns currentILogConfiguration.</returns>
        ILogConfiguration UseSeq(LogLevel logLevel = LogLevel.INFO);

        /// <summary>
        /// Creates Logger from the LogConfiguration.
        /// </summary>
        /// <returns> It returns current ILogConfiguration. </returns>
        ILogConfiguration Initiate();
    }
}

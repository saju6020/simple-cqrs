namespace Platform.Infrastructure.LogConfiguration.SerilogConfiguration
{
    using System;
    using Serilog.Events;
    using Platform.Infrastructure.LogConfiguration.Abstraction;

    /// <summary>Serilog log level provider.</summary>
    public class SeriLogLevelProvider
    {
        /// <summary>Gets the log level.</summary>
        /// <param name="logLevel">The log level.</param>
        /// <returns>Log level.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If no log level present then throw exception.</exception>
        public static LogEventLevel GetLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.ALL:
                    return LogEventLevel.Verbose;
                case LogLevel.DEBUG:
                    return LogEventLevel.Debug;
                case LogLevel.INFO:
                    return LogEventLevel.Information;
                case LogLevel.WARN:
                    return LogEventLevel.Warning;
                case LogLevel.ERROR:
                    return LogEventLevel.Error;
                case LogLevel.FATAL:
                    return LogEventLevel.Fatal;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }
    }
}

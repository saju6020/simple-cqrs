namespace Platform.Infrastructure.LogConfiguration.SerilogConfiguration
{
    using System;
    using Platform.Infrastructure.CustomException;
    using Platform.Infrastructure.LogConfiguration.Abstraction;

    public class InternalLogLevelProvider
    {
        public static LogLevel GetInternalLogLevel(string logLevel)
        {
            try
            {
                switch (logLevel.ToUpper())
                {
                    case Constants.ALL:
                        return LogLevel.ALL;

                    case Constants.DEBUG:
                        return LogLevel.DEBUG;

                    case Constants.INFO:
                        return LogLevel.INFO;

                    case Constants.WARN:
                        return LogLevel.WARN;

                    case Constants.ERROR:
                        return LogLevel.ERROR;

                    case Constants.FATAL:
                        return LogLevel.FATAL;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(logLevel));
                }
            }
            catch (Exception ex)
            {
                throw new BaseException(ex.Message);
            }
        }
    }
}

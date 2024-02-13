namespace Platform.Infrastructure.LogConfiguration.SerilogConfiguration
{
    using Platform.Infrastructure.LogConfiguration.Abstraction;

    /// <summary>Configuration service to register logger for the project.</summary>
    public static class SeriLogConfigurationService
    {
        /// <summary>Adds the seri log.</summary>
        /// <param name="logSettings">The log settings.</param>
        /// <returns>log configuration.</returns>
        public static ILogConfiguration AddSeriLog(this LogSettings logSettings)
        {
            return new SeriLogConfiguration { LogSettings = logSettings };
        }
    }
}

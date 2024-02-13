namespace Platform.Infrastructure.LogConfiguration.Abstraction
{
    using Platform.Infrastructure.AppConfigurationManager;

    /// <summary>This class is to add log configuration.</summary>
    public class LogConfigurationService
    {
        /// <summary>Adds the configuration.</summary>
        /// <param name="configurationManager">Param configuration manager will be passed from caller.</param>
        /// <returns>Log settings.</returns>
        public static LogSettings AddConfiguration(IAppConfigurationManager configurationManager)
        {
            return new LogSettingsInitializer(configurationManager).Initialize();
        }
    }
}

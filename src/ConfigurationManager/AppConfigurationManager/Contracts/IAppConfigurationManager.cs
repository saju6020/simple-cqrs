namespace Platform.Infrastructure.AppConfigurationManager
{
    /// <summary>App configuration manager interface.</summary>
    public interface IAppConfigurationManager
    {
        /// <summary>Gets the global settings.</summary>
        /// <value>The global settings.</value>
        GlobalSettings GlobalSettings { get; }

        /// <summary>Gets the application settings.</summary>
        /// <value>The application settings.</value>
        IAppSettings AppSettings { get; }

        /// <summary>Gets the application settings provider.</summary>
        /// <returns>Get app settings provider.</returns>
        IAppSettingsProvider GetAppSettingsProvider();

        /// <summary>Uses the azure based application configuration.</summary>
        /// <returns>App configuration manager.</returns>
        IAppConfigurationManager UseAzureBasedAppConfiguration();

        /// <summary>Uses the file based application configuration.</summary>
        /// <returns>App configuration manager.</returns>
        IAppConfigurationManager UseFileBasedAppConfiguration();
    }
}
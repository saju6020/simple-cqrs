namespace Platform.Infrastructure.AppConfigurationManager
{
    /// <summary>This class will allow user to include their appsettings through template to merge with the default settings.</summary>
    /// <typeparam name="T">User defined appsettings.</typeparam>
    /// <seealso cref="Shohoz.Platform.Infrastructure.AppConfigurationManager.IAppConfigurationManager{T}" />
    public class AppConfigurationManager<T> : IAppConfigurationManager<T>
        where T : IAppSettings
    {
        public AppConfigurationManager(IGlobalSettingsProvider globalSettingsProvider, IAppSettingsProvider appSettingsProvider)
        {
            this.AppSettingsProvider = appSettingsProvider;

            this.GlobalSettingsProvider = globalSettingsProvider;
        }

        /// <summary>Gets the global settings.</summary>
        /// <value>The global settings.</value>
        public GlobalSettings GlobalSettings { get; internal set; }

        /// <summary>Gets the application settings.</summary>
        /// <value>The application settings.</value>
        public T AppSettings { get; internal set; }

        /// <summary>Gets or sets the application settings provider.</summary>
        /// <value>The application settings provider.</value>
        internal IAppSettingsProvider AppSettingsProvider { get; set; }

        /// <summary>Gets or sets the application settings provider.</summary>
        /// <value>The application settings provider.</value>
        internal IGlobalSettingsProvider GlobalSettingsProvider { get; set; }

        /// <summary>Gets the application settings provider.</summary>
        /// <returns>Return app settings provider.</returns>
        public IAppSettingsProvider GetAppSettingsProvider()
        {
            return this.AppSettingsProvider;
        }

        /// <summary>Uses the azure based application configuration.</summary>
        /// <returns>Return azure based app configuration.</returns>
        public IAppConfigurationManager<T> UseAzureBasedAppConfiguration()
        {
            var azureBasedAppConfigManager = new AzureBasedAppConfigurationManager<T>(this.GlobalSettingsProvider, this.AppSettingsProvider);
            this.AppSettings = azureBasedAppConfigManager.AppSettings;
            this.GlobalSettings = azureBasedAppConfigManager.GlobalSettings;
            this.AppSettingsProvider = azureBasedAppConfigManager.AppSettingsProvider;
            return this;
        }

        /// <summary>Uses the file based application configuration.</summary>
        /// <returns>Return file base app configuration.</returns>
        public IAppConfigurationManager<T> UseFileBasedAppConfiguration()
        {
            var fileBasedAppConfigManager = new FileBasedAppConfigurationManager<T>(this.GlobalSettingsProvider, this.AppSettingsProvider);
            this.AppSettings = fileBasedAppConfigManager.AppSettings;
            this.GlobalSettings = fileBasedAppConfigManager.GlobalSettings;
            this.AppSettingsProvider = fileBasedAppConfigManager.AppSettingsProvider;
            return this;
        }
    }
}

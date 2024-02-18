namespace Platform.Infrastructure.AppConfigurationManager
{
    /// <summary>This class is to manage app configuration.</summary>
    /// <seealso cref="Platform.Infrastructure.AppConfigurationManager.IAppConfigurationManager" />
    public class AppConfigurationManager : IAppConfigurationManager
    {
        /// <summary>Initializes a new instance of the <see cref="AppConfigurationManager"/> class.</summary>
        public AppConfigurationManager(IGlobalSettingsProvider globalSettingsProvider, IAppSettingsProvider appSettingsProvider)
        {
            this.AppSettingsProvider = appSettingsProvider;

            this.GlobalSettingsProvider = globalSettingsProvider;

            AppConfigurationManager<DefaultAppSettings> defaultConfigManager = new AppConfigurationManager<DefaultAppSettings>(this.GlobalSettingsProvider, this.AppSettingsProvider);

            this.AppSettingsProvider = defaultConfigManager.GetAppSettingsProvider();

            this.GlobalSettings = defaultConfigManager.GlobalSettings;

            this.AppSettings = defaultConfigManager.AppSettings;
        }

        /// <summary>Gets the global settings.</summary>
        /// <value>The global settings.</value>
        public GlobalSettings GlobalSettings { get; internal set; }

        /// <summary>Gets the application settings.</summary>
        /// <value>The application settings.</value>
        public IAppSettings AppSettings { get; internal set; }

        /// <summary>Gets or sets the application settings provider.</summary>
        /// <value>The application settings provider.</value>
        internal IAppSettingsProvider AppSettingsProvider { get; set; }

        /// <summary>Gets or sets the application settings provider.</summary>
        /// <value>The application settings provider.</value>
        internal IGlobalSettingsProvider GlobalSettingsProvider { get; set; }

        /// <summary>Gets the application settings provider.</summary>
        /// <returns>Get app settings provider.</returns>
        public IAppSettingsProvider GetAppSettingsProvider()
        {
            return this.AppSettingsProvider;
        }

        /// <summary>Uses the azure based application configuration.</summary>
        /// <returns>App configuration manager.</returns>
        public IAppConfigurationManager UseAzureBasedAppConfiguration()
        {
            var azureBasedAppConfigManager = new AzureBasedAppConfigurationManager<DefaultAppSettings>(this.GlobalSettingsProvider, this.AppSettingsProvider);
            this.AppSettings = azureBasedAppConfigManager.AppSettings;
            this.GlobalSettings = azureBasedAppConfigManager.GlobalSettings;
            this.AppSettingsProvider = azureBasedAppConfigManager.AppSettingsProvider;
            return this;
        }

        /// <summary>Uses the file based application configuration.</summary>
        /// <returns>App configuration manager.</returns>
        public IAppConfigurationManager UseFileBasedAppConfiguration()
        {
            var fileBasedAppConfigManager = new FileBasedAppConfigurationManager<DefaultAppSettings>(this.GlobalSettingsProvider, this.AppSettingsProvider);
            this.AppSettings = fileBasedAppConfigManager.AppSettings;
            this.GlobalSettings = fileBasedAppConfigManager.GlobalSettings;
            this.AppSettingsProvider = fileBasedAppConfigManager.AppSettingsProvider;
            return this;
        }
    }
}

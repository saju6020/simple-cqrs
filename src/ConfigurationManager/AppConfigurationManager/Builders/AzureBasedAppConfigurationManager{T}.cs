namespace Platform.Infrastructure.AppConfigurationManager
{
    using System.Text.Json;

    /// <summary>This class will allow user to include their appsettings through template to merge with the default settings.</summary>
    /// <typeparam name="T">User defined appsettings.</typeparam>
    /// <seealso cref="Shohoz.Core.Infrastructure.AppConfigurationManager.IAppConfigurationManager{T}" />
    public class AzureBasedAppConfigurationManager<T> : IAppConfigurationManager<T>
        where T : IAppSettings
    {
        /// <summary>Initializes a new instance of the <see cref="AzureBasedAppConfigurationManager{T}"/> class.</summary>
        public AzureBasedAppConfigurationManager(IGlobalSettingsProvider globalSettingsProvider, IAppSettingsProvider appSettingsProvider)
        {
            this.GlobalSettingsProvider = globalSettingsProvider;

            this.AppSettingsProvider = appSettingsProvider;

            this.GlobalSettings = this.GetGlobalSettings();

            this.AppSettingsProvider.SetGlobalAndAppSettingsFromAzureAppConfiguration(this.GlobalSettings.AzureGlobalAppConfigConnectionString);

            this.GlobalSettings = this.AppSettingsProvider.GetGlobalSettingsFromConfiguration();

            this.AppSettings = this.AppSettingsProvider.GetAppSettingsFromConfiguration<T>();
        }

        /// <summary>Gets the application settings.</summary>
        /// <value>The application settings.</value>
        public T AppSettings { get; internal set; }

        /// <summary>Gets the global settings.</summary>
        /// <value>The global settings.</value>
        public GlobalSettings GlobalSettings { get; internal set; }

        /// <summary>Gets or sets the application settings provider.</summary>
        /// <value>The application settings provider.</value>
        internal IAppSettingsProvider AppSettingsProvider { get; set; }

        /// <summary>Gets or sets the GlobalSettingsProvider provider.</summary>
        /// <value>The GlobalSettingsProvider provider.</value>
        internal IGlobalSettingsProvider GlobalSettingsProvider { get; set; }

        /// <summary>Gets the application settings provider.</summary>
        /// <returns>Return app settings provider.</returns>
        public IAppSettingsProvider GetAppSettingsProvider()
        {
            return this.AppSettingsProvider;
        }

        /// <summary>Gets the global settings.</summary>
        /// <returns>Return global settings.</returns>
        private GlobalSettings GetGlobalSettings()
        {
            if (this.GlobalSettings != null)
            {
                return this.GlobalSettings;
            }

            var serviceName = this.AppSettingsProvider.GetValue(ApplicationConstants.ServiceNameKey);

            var globalConfigJsonPath = this.AppSettingsProvider.GetValue(ApplicationConstants.GlobalSettingsJsonPathKey);

            this.GlobalSettings = this.GlobalSettingsProvider.GetGlobalSettings(serviceName, globalConfigJsonPath);

            return this.GlobalSettings;
        }
    }
}

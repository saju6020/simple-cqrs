namespace Platform.Infrastructure.AppConfigurationManager
{
    using System.Text.Json;

    /// <summary>This class will allow user to include their appsettings through template to merge with the default settings.</summary>
    /// <typeparam name="T">User defined appsettings.</typeparam>
    /// <seealso cref="Shohoz.Platform.Infrastructure.AppConfigurationManager.IAppConfigurationManager{T}" />
    public class FileBasedAppConfigurationManager<T> : IAppConfigurationManager<T>
        where T : IAppSettings
    {
        /// <summary>Initializes a new instance of the <see cref="FileBasedAppConfigurationManager{T}"/> class.</summary>
        public FileBasedAppConfigurationManager(IGlobalSettingsProvider globalSettingsProvider, IAppSettingsProvider appSettingsProvider)
        {
            this.GlobalSettingsProvider = globalSettingsProvider;

            this.AppSettingsProvider = appSettingsProvider;

            this.GlobalSettings = this.GetGlobalSettings();

            this.SetAppSettingsType();
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

        /// <summary>Sets the type of the application settings.</summary>
        private void SetAppSettingsType()
        {
            this.AppSettings = this.AppSettingsProvider.GetAppSettingsFromConfiguration<T>();
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

            this.GlobalSettings = this.AppSettingsProvider.ReplaceGlobalSettingsWithAppSetting(this.GlobalSettings);

            return this.GlobalSettings;
        }
    }
}

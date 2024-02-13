namespace Platform.Infrastructure.AppConfigurationManager
{
    /// <summary>Generic app configuration manager abstraction.</summary>
    /// <typeparam name="T">User define app settings.</typeparam>
    public interface IAppConfigurationManager<T>
        where T : IAppSettings
    {
        /// <summary>Gets the global settings.</summary>
        /// <value>The global settings.</value>
        GlobalSettings GlobalSettings { get; }

        /// <summary>Gets the application settings.</summary>
        /// <value>The application settings.</value>
        T AppSettings { get; }

        /// <summary>Gets the application settings provider.</summary>
        /// <returns>App settings provider.</returns>
        IAppSettingsProvider GetAppSettingsProvider();
    }
}
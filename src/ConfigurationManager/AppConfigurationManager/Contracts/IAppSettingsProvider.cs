namespace Platform.Infrastructure.AppConfigurationManager
{
    using Microsoft.Extensions.Configuration;

    /// <summary>App settings provider abstraction.</summary>
    public interface IAppSettingsProvider
    {
        /// <summary>Gets the configuration root.</summary>
        /// <value>The configuration root.</value>
        IConfigurationRoot ConfigurationRoot { get; }

        /// <summary>Gets the value.</summary>
        /// <param name="key">The key.</param>
        /// <returns>Return value based on key.</returns>
        string GetValue(string key);

        GlobalSettings ReplaceGlobalSettingsWithAppSetting(GlobalSettings globalSettings);

        void SetGlobalAndAppSettingsFromAzureAppConfiguration(string azureGlobalAppConfigConnectionString);

        GlobalSettings GetGlobalSettingsFromConfiguration();

        T GetAppSettingsFromConfiguration<T>();
    }
}

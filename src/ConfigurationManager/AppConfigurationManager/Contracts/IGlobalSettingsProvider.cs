namespace Platform.Infrastructure.AppConfigurationManager
{
    using Microsoft.Extensions.Configuration;

    /// <summary>App settings provider abstraction.</summary>
    public interface IGlobalSettingsProvider
    {
        /// <summary>Gets global settings.</summary>
        /// <value>servicename and globalsettings json path.</value>
        /// <returns>Returns global settings</returns>
        GlobalSettings GetGlobalSettings(string serviceName, string globalSettingsJsonPath);
    }
}

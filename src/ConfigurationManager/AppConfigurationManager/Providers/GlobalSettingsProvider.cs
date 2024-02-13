namespace Platform.Infrastructure.AppConfigurationManager
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using Platform.Infrastructure.CustomException;

    /// <summary>Global settings provider to provide data from global settings file.</summary>
    public class GlobalSettingsProvider : IGlobalSettingsProvider
    {
        /// <summary>Gets the global settings.</summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="globalSettingsJsonPath">The global settings json path.</param>
        /// <returns>Return global settings.</returns>
        /// <exception cref="KeyNotFoundException">Return key not found exception.</exception>
        public GlobalSettings GetGlobalSettings(string serviceName, string globalSettingsJsonPath)
        {
            if (string.IsNullOrEmpty(globalSettingsJsonPath))
            {
                var message = $"{ApplicationConstants.GlobalSettingsJsonPathKey} is not defined in app/web settings file please add it in following way\n <add key=\"{ApplicationConstants.GlobalSettingsJsonPathKey}\" value=\"C:\\SHOHOZ_CONFIGS\\GlobalConfig.json\"/>";
                throw new KeyNotFoundException(message);
            }

            var globalSettings = this.CreateGlobalConfigFromJson(serviceName, globalSettingsJsonPath);

            return globalSettings;
        }

        /// <summary>Creates the global configuration from json.</summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="globalSettingsJsonPath">The global settings json path.</param>
        /// <returns>Return global settings.</returns>
        /// <exception cref="FileIsEmptyException">Return exception if file is empty.</exception>
        private GlobalSettings CreateGlobalConfigFromJson(string serviceName, string globalSettingsJsonPath)
        {
            var globalSettingsJson = File.ReadAllText(globalSettingsJsonPath);

            if (string.IsNullOrEmpty(globalSettingsJson))
            {
                throw new FileIsEmptyException($"{globalSettingsJsonPath} File is empty");
            }

            var globalSettings = JsonSerializer.Deserialize<GlobalSettings>(globalSettingsJson);

            globalSettings.ServiceName = serviceName;

            return globalSettings;
        }
    }
}

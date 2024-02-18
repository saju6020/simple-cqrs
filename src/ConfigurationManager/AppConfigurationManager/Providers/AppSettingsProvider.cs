namespace Platform.Infrastructure.AppConfigurationManager
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.AzureAppConfiguration;
    using Newtonsoft.Json.Linq;
    using Platform.Infrastructure.CustomException;

    /// <summary>This class provide the app settings.</summary>
    /// <seealso cref="Platform.Infrastructure.AppConfigurationManager.IAppSettingsProvider" />
    public class AppSettingsProvider : IAppSettingsProvider
    {
        /// <summary>Gets the configuration root.</summary>
        /// <value>The configuration root.</value>
        public IConfigurationRoot ConfigurationRoot { get; internal set; }

        public AppSettingsProvider()
        {
            this.ConfigurationRoot = this.BuildAppsettings();
        }

        /// <summary>Gets the value.</summary>
        /// <param name="key">The key.</param>
        /// <returns>Return value based on key from configuration root.</returns>
        public string GetValue(string key)
        {
            return this.ConfigurationRoot[key];
        }

        /// <summary>Replaces the global settings with application setting.</summary>
        /// <param name="globalSettings">The global settings.</param>
        /// <returns>Return overriden global settings by appsettings value.</returns>
        public GlobalSettings ReplaceGlobalSettingsWithAppSetting(GlobalSettings globalSettings)
        {
            var golbalConfigJsonObject = JObject.FromObject(globalSettings);

            foreach (var section in this.ConfigurationRoot.GetChildren())
            {
                if (typeof(GlobalSettings).GetProperty(section.Key) != null)
                {
                    golbalConfigJsonObject[section.Key] = section.Value;
                }
            }

            return golbalConfigJsonObject.ToObject<GlobalSettings>();
        }

        /// <summary>Sets the global and application settings from azure application configuration.</summary>
        /// <param name="azureGlobalAppConfigConnectionString">The azure global application configuration connection string.</param>
        public void SetGlobalAndAppSettingsFromAzureAppConfiguration(string azureGlobalAppConfigConnectionString)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            configurationBuilder = configurationBuilder.AddConfiguration(this.ConfigurationRoot);

            configurationBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(azureGlobalAppConfigConnectionString);
            });

            this.ConfigurationRoot = configurationBuilder.Build();

            configurationBuilder = configurationBuilder.AddConfiguration(this.ConfigurationRoot);

            configurationBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(this.ConfigurationRoot[ApplicationConstants.AzureAppConfigConnectionString])
                    .Select(KeyFilter.Any, LabelFilter.Null);

                // .ConfigureRefresh(refreshoption => { refreshoption.Register("RefresherCount", refreshAll: true).SetCacheExpiration(TimeSpan.FromSeconds(10)); });

                // ConfigurationRefresher = options.GetRefresher();
            });

            this.ConfigurationRoot = configurationBuilder.Build();
        }

        /// <summary>Gets the global settings from configuration.</summary>
        /// <returns>Return global settings.</returns>
        public GlobalSettings GetGlobalSettingsFromConfiguration()
        {
            var golbalConfigJsonObject = new JObject();

            foreach (var section in this.ConfigurationRoot.GetChildren())
            {
                if (typeof(GlobalSettings).GetProperty(section.Key) != null)
                {
                    golbalConfigJsonObject[section.Key] = section.Value;
                }
            }

            return golbalConfigJsonObject.ToObject<GlobalSettings>();
        }

        /// <summary>Gets the application settings from configuration.</summary>
        /// <typeparam name="T">param template.</typeparam>
        /// <returns>Return passed type.</returns>
        public T GetAppSettingsFromConfiguration<T>()
        {
            var golbalConfigJsonObject = new JObject();

            foreach (var section in this.ConfigurationRoot.GetChildren())
            {
                if (typeof(T).GetProperty(section.Key) != null)
                {
                    golbalConfigJsonObject[section.Key] = section.Value;
                }
            }

            return golbalConfigJsonObject.ToObject<T>();
        }

        private IConfigurationRoot BuildAppsettings()
        {
            try
            {
                var configurationBuilder = new ConfigurationBuilder();

                configurationBuilder.AddEnvironmentVariables();

                var currentEnvironment = Environment.GetEnvironmentVariable(EnvironmentConstants.AspnetcoreEnvironmentVariableName);

                var currentAppSettingsFileName = string.IsNullOrWhiteSpace(currentEnvironment) ? "appsettings.json" : $"appsettings.{currentEnvironment}.json";

                configurationBuilder.AddJsonFile(currentAppSettingsFileName, optional: false, reloadOnChange: false);

                return configurationBuilder.Build();
            }
            catch (FailedToBuildAppSettingsException)
            {
                throw;
            }
        }
    }
}

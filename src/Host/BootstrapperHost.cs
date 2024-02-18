namespace Platform.Infrastructure.Host
{
    using Microsoft.Extensions.DependencyInjection;
    using Platform.Infrastructure.AppConfigurationManager;
    using Platform.Infrastructure.Host.Contracts;
    using Platform.Infrastructure.LogConfiguration.Abstraction;
    using Platform.Infrastructure.LogConfiguration.SerilogConfiguration;

    /// <summary>Basic bootstrapper class</summary>
    /// <seealso cref="IBootstrapper" />
    public class BootstrapperHost : IBootstrapper
    {
        /// <summary>Gets the application configuration manager.</summary>
        /// <value>The application configuration manager.</value>
        public IAppConfigurationManager AppConfigurationManager { get; internal set; }

        /// <summary>Gets the application configuration manager.</summary>
        /// <value>The application configuration manager.</value>
        public ILogConfiguration LogConfiguration { get; internal set; }

        private IAppSettingsProvider AppSettingsProvider { get; set; }

        private IGlobalSettingsProvider GlobalSettingsProvider { get; set; }

        public BootstrapperHost()
        {
            this.AppSettingsProvider = new AppSettingsProvider();
            this.GlobalSettingsProvider = new GlobalSettingsProvider();
            this.AppConfigurationManager = new AppConfigurationManager(this.GlobalSettingsProvider, this.AppSettingsProvider);
        }

        public BootstrapperHost(IGlobalSettingsProvider globalSettingsProvider)
        {
            this.AppSettingsProvider = new AppSettingsProvider();
            this.GlobalSettingsProvider = globalSettingsProvider;
            this.AppConfigurationManager = new AppConfigurationManager(this.GlobalSettingsProvider, this.AppSettingsProvider);
        }

        /// <summary>Adds the core services.</summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="serviceConfig">The service configuration.</param>
        public virtual IBootstrapper AddCoreServices(IServiceCollection serviceCollection, BootstrapperServiceConfig serviceConfig)
        {
            serviceCollection.AddSingleton<IAppSettingsProvider>(this.AppSettingsProvider);
            serviceCollection.AddSingleton<IGlobalSettingsProvider>(this.GlobalSettingsProvider);
            serviceCollection.AddSingleton<IAppConfigurationManager>(this.AppConfigurationManager);

            if (serviceConfig.UseLocalization)
            {
               // serviceCollection.UseLocalization();
            }

            return this;
        }

        /// <summary>Uses the azure based application configuration.</summary>
        /// <returns>App configuration manager.</returns>
        public IBootstrapper UseAzureBasedAppConfiguration()
        {
            this.AppConfigurationManager = this.AppConfigurationManager.UseAzureBasedAppConfiguration();

            this.ConfigureLogger();

            return this;
        }

        /// <summary>Uses the file based application configuration.</summary>
        /// <returns>App configuration manager.</returns>
        public IBootstrapper UseFileBasedAppConfiguration()
        {
            this.AppConfigurationManager = this.AppConfigurationManager.UseFileBasedAppConfiguration();

            this.ConfigureLogger();

            return this;
        }

        public void Init() 
        {
            LogConfiguration.Initiate();
        }

        private void ConfigureLogger()
        {
            LogConfiguration = LogConfigurationService
                .AddConfiguration(this.AppConfigurationManager)
                .AddSeriLog()
                .UseConsole();
        }
    }
}

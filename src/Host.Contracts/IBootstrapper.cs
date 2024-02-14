namespace Platform.Infrastructure.Host.Contracts
{
    using Microsoft.Extensions.DependencyInjection;
    using Platform.Infrastructure.AppConfigurationManager;
    using Platform.Infrastructure.LogConfiguration.Abstraction;

    /// <summary>Boot strapper abstraction</summary>
    public interface IBootstrapper
    {
        /// <summary>Gets the application configuration manager.</summary>
        /// <value>The application configuration manager.</value>
        IAppConfigurationManager AppConfigurationManager { get; }

        ILogConfiguration LogConfiguration { get; }

        /// <summary>Adds the core services.</summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="serviceConfig">The service configuration.</param>
        IBootstrapper AddCoreServices(IServiceCollection serviceCollection, BootstrapperServiceConfig serviceConfig);

        /// <summary>Uses the azure based application configuration.</summary>
        /// <returns>App configuration manager.</returns>
        IBootstrapper UseAzureBasedAppConfiguration();

        /// <summary>Uses the file based application configuration.</summary>
        /// <returns>App configuration manager.</returns>
        IBootstrapper UseFileBasedAppConfiguration();

        /// <summary>Uses the file based application configuration.</summary>
        /// <returns>App configuration manager.</returns>
        void Init();
    }
}

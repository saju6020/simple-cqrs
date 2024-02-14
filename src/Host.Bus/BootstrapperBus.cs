namespace Platform.Infrastructure.Host.Bus
{
    using global::Platform.Infrastructure.Host.Contracts;
    using global::Platform.Infrastructure.Host.WebApi;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Platform.Infrastructure.Host.Contracts;
    using Platform.Infrastructure.Host.WebApi;

    /// <summary>Class to bootstrap a web api with service bus functionality enabled</summary>
    /// <seealso cref="Shohoz.Platform.Infrastructure.Host.WebApi.BootstrapperWebApi" />
    public class BootstrapperBus : BootstrapperWebApi
    {
        /// <summary>Builds the pipe line.</summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Return application builder.</returns>
        public override IApplicationBuilder BuildPipeLine(IApplicationBuilder applicationBuilder, BootstrapperPipeLineConfig config) 
        {
            return base.BuildPipeLine(applicationBuilder, config);
        }

        /// <summary>Adds the core services.</summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="serviceConfig">The service configuration.</param>
        public override IBootstrapper AddCoreServices(IServiceCollection serviceCollection, BootstrapperServiceConfig serviceConfig)
        {
            base.AddCoreServices(serviceCollection, serviceConfig);

            return this;
        }
    }
}

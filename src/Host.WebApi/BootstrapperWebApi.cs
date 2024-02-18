namespace Platform.Infrastructure.Host.WebApi
{
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Platform.Infrastructure.AppConfigurationManager;
    using Platform.Infrastructure.EndpointRoleFeatureMap.Extensions;
    using Platform.Infrastructure.Host.Contracts;
    using Platform.Infrastructure.Host.WebApi.Extensions;
    using Serilog;
    using AppConfigurationManager = Platform.Infrastructure.AppConfigurationManager.AppConfigurationManager;

    /// <summary>This class to bootstrap web api with proper middleware and service registration.</summary>
    /// <seealso cref="Platform.Infrastructure.Host.BootstrapperHost" />
    /// <seealso cref="Platform.Infrastructure.Host.WebApi.IBootstrapperWeb" />
    public class BootstrapperWebApi : BootstrapperHost, IBootstrapperWeb
    {
        public BootstrapperWebApi()
        {
        }

        public BootstrapperWebApi(IGlobalSettingsProvider globalSettingsProvider)
            : base(globalSettingsProvider)
        {
        }

        /// <summary>Builds the pipe line.</summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <param name="pipeLineConfig">The pipe line configuration.</param>
        /// <returns>Application builder.</returns>
        public virtual IApplicationBuilder BuildPipeLine(IApplicationBuilder applicationBuilder, BootstrapperPipeLineConfig pipeLineConfig)
        {
            return applicationBuilder
                .UseSerilogRequestLogging()
                .UseAzureAppConfiguration()
                .UseHttpPipeline(
                    pipeLineConfig.EnableAuthorization,
                    pipeLineConfig.EnableServiceIdCheckerMiddleware,
                    pipeLineConfig.EnableVerticalIdCheckerMiddleware);
        }

        /// <summary>Adds the core services.</summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="serviceConfig">The service configuration.</param>
        /// <returns>Returns IBootstrapper.</returns>
        public override IBootstrapper AddCoreServices(IServiceCollection serviceCollection, BootstrapperServiceConfig serviceConfig)
        {
            base.AddCoreServices(serviceCollection, serviceConfig);

            IMvcCoreBuilder mvcBuilder = serviceCollection.AddHttpComponents();

            if (serviceConfig.UseEndpointProtection)
            {
                mvcBuilder.AddEndpointSecurityPolicies();
            }

            if (serviceConfig.UseEndPointMfaProtection)
            {
                mvcBuilder.AddEndpointMfaPolicies();
            }

            if (serviceConfig.UseEndPoint2faProtection)
            {
                mvcBuilder.AddEndpoint2faPolicies();
            }

            if (serviceConfig.UseEndPointHashProtection)
            {
                mvcBuilder.AddEndpointHashPolicies();
            }

            return this;
        }
    }
}

namespace Platform.Infrastructure.Host.WebApi.Extensions
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.DependencyInjection;
    using Platform.Infrastructure.Host.Contracts;
    using System.Text.Json.Serialization;
    using Platform.Infrastructure.EndpointRoleFeatureMap.Extensions;

    /// <summary>Class to contain service collection extensions methods.</summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds the authorization policies.</summary>
        /// <param name="serviceCollection">The service collection.</param>
        public static void AddAuthorizationPolicies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAuthorizationCore(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
            });
        }

        public static IMvcCoreBuilder AddHttpComponents(this IServiceCollection services)
        {
            services.AddRouting();

            services.AddHealthChecks();

            return services.AddMvcCore((options) =>
            {
            })
            .AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
                jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddNewtonsoftJson()
            .AddCors();
        }

        public static IServiceCollection  AddCoreServices(this IServiceCollection serviceCollection, HostServiceConfig serviceConfig)
        {

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

            return serviceCollection;
           
        }

    }
}

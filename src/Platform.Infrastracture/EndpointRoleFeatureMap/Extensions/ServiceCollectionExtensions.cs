namespace Platform.Infrastructure.EndpointRoleFeatureMap.Extensions
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.DependencyInjection;
    using Platform.Infrastructure.EndpointRoleFeatureMap.AuthorizationHandlers;
    using Platform.Infrastructure.EndpointRoleFeatureMap.Models;
    using Platform.Infrastructure.EndpointRoleFeatureMap.Providers;

    public static class ServiceCollectionExtensions
    {
        public static void AddEndpointSecurityPolicies(this IMvcCoreBuilder mvcCoreBuilder)
        {
            mvcCoreBuilder.Services.AddSingleton<EndpointRoleFeatureMapProvider>();
            mvcCoreBuilder.Services.AddSingleton<IAuthorizationHandler, ProtectedEndpointAccessHandler>();

            mvcCoreBuilder.AddAuthorization(options =>
            {
                options.AddPolicy(ApiProtectionType.Protected, policy =>
                    policy.Requirements.Add(new ProtectedEndpointAccessRequirement()));
            });

        }

        public static void AddEndpointMfaPolicies(this IMvcCoreBuilder mvcCoreBuilder)
        {
            mvcCoreBuilder.Services.AddSingleton<IAuthorizationHandler, RequireMfaHandler>();
            mvcCoreBuilder.AddAuthorization(options =>
            {
                options.AddPolicy(ApiProtectionType.MfaProtected, policy =>
                policy.Requirements.Add(new RequireMfa()));
            });
        }

        public static void AddEndpoint2faPolicies(this IMvcCoreBuilder mvcCoreBuilder)
        {
            mvcCoreBuilder.Services.AddSingleton<IAuthorizationHandler, Require2faHandler>();
            mvcCoreBuilder.AddAuthorization(options =>
            {
                options.AddPolicy(ApiProtectionType.TwofaProtected, policy =>
                    policy.Requirements.Add(new Require2fa()));
            });
        }

        public static void AddEndpointHashPolicies(this IMvcCoreBuilder mvcCoreBuilder)
        {
            mvcCoreBuilder.Services.AddSingleton<IAuthorizationHandler, RequireHashHandler>();
            mvcCoreBuilder.AddAuthorization(options =>
            {
                options.AddPolicy(ApiProtectionType.HashProtected, policy =>
                    policy.Requirements.Add(new RequireHash()));
            });
        }
    }
}

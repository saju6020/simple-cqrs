// <copyright file="ServiceCollectionExtensions.cs" company="Shohoz">
// Copyright © 2015-2020  All Rights Reserved.
// </copyright>

namespace Platform.Infrastructure.Host.WebApi.Extensions
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.DependencyInjection;

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
    }
}

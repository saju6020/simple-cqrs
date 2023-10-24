namespace Validation.FluentValidationProvider.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Platform.Infrastructure.Core.Extensions;
    using Platform.Infrastructure.Core.Validation;
    using Shohoz.Platform.Infrastructure.Validation.FluentValidationProvider;

    /// <summary>Class to contain extension method related to fluent validation.</summary>
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddFluentValidation(this IServiceCollection services, Action<ValidationOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.Configure(configureOptions);

            services.AddScoped<IValidationProvider, FluentValidationProvider>();
            services.AddScoped<IValidationService, ValidationService>();

            return services;
        }
    }
}

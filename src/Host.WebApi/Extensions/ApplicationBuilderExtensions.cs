using System;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Platform.Infrastructure.AppConfigurationManager;
using Platform.Infrastructure.Host.WebApi.Middlewares;

namespace Platform.Infrastructure.Host.WebApi.Extensions
{
    /// <summary>Class to contain application builder extensions.</summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>Uses the cors to allow all.</summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <returns>Application builder.</returns>
        public static IApplicationBuilder UseCorsToAllowAll(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseCors((corsPolicyBuilder) =>
                corsPolicyBuilder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((string origin) => true)
                .AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromDays(365)));
            return applicationBuilder;
        }

        /// <summary>Uses global exception handler.</summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <returns>Application builder.</returns>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            return applicationBuilder;
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Platform.Infrastructure.Host.WebApi.Middlewares;
using System;

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
       

        public static IApplicationBuilder UseHttpPipeline(
           this IApplicationBuilder applicationBuilder,
           bool enableAuthorization = false,
           bool enableServiceIdCheckerMiddleware = false,
           bool enableTenantIdCheckerMiddleware = false)
        {
            if (enableServiceIdCheckerMiddleware)
            {
                applicationBuilder.UseMiddleware<ServiceIdHeaderMiddleware>();
            }

            if (enableTenantIdCheckerMiddleware)
            {
                applicationBuilder.UseMiddleware<TenantIdHeaderMiddleware>();
            }

            applicationBuilder.UseRouting();

            applicationBuilder.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            applicationBuilder.UseCors((corsPolicyBuilder) =>
                   corsPolicyBuilder
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .SetIsOriginAllowed((string origin) => true)
                   .AllowCredentials()
                   .SetPreflightMaxAge(TimeSpan.FromDays(365)));

            if (enableAuthorization)
            {
                applicationBuilder.UseAuthentication();
                applicationBuilder.UseAuthorization();
            }

            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "Default",
                   pattern: "{controller}/{action}/{id?}");
                endpoints.MapHealthChecks("/health");
            });

            return applicationBuilder;
        }

      
        private static void UseRoutingAndCors(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseRouting();

            applicationBuilder.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            applicationBuilder.UseCors((corsPolicyBuilder) =>
                   corsPolicyBuilder
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .SetIsOriginAllowed((string origin) => true)
                   .AllowCredentials()
                   .SetPreflightMaxAge(TimeSpan.FromDays(365)));
        }
    }
}

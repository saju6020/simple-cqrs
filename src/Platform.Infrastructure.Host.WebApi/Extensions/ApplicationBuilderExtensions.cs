namespace Platform.Infrastructure.Host.WebApi.Extensions
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Platform.Infrastructure.Host.WebApi.Middlewares;

    /// <summary>Class to contain application builder extensions.</summary>
    public static class ApplicationBuilderExtensions
    {        
        public static IApplicationBuilder UseCorsToAllowAll(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseCorsToAllowAll();
            return applicationBuilder;
        }

        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            return applicationBuilder;
        }
    }
}

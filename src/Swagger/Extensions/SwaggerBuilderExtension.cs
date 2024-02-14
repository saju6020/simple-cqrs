// <copyright file="SwaggerAuthorizeExtensions.cs" company="Shohoz">
// Copyright © 2015-2021 Shohoz. All Rights Reserved.
// </copyright>

namespace Shohoz.Platform.Infrastructure.Swagger.Extensions
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Builder;
    using Shohoz.Platform.Infrastructure.Swagger.Middlewares;

    [ExcludeFromCodeCoverage]
    public static class SwaggerBuilderExtension
    {
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<SwaggerBasicAuthMiddleware>();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            builder.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });

            return builder;
        }
    }
}

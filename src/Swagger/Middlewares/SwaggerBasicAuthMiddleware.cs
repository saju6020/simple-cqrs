// <copyright file="SwaggerBasicAuthMiddleware.cs" company="Shohoz">
// Copyright © 2015-2021 Shohoz. All Rights Reserved.
// </copyright>

namespace Shohoz.Platform.Infrastructure.Swagger.Middlewares
{
    using System;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Shohoz.Platform.Infrastructure.Swagger.Configs;

    public class SwaggerBasicAuthMiddleware
    {
        private readonly RequestDelegate next;
        private readonly SwaggerAuthConfig swaggerAuthConfig;

        public SwaggerBasicAuthMiddleware(RequestDelegate next, IOptions<SwaggerAuthConfig> swaggerAuthConfig)
        {
            this.next = next;
            this.swaggerAuthConfig = swaggerAuthConfig.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Make sure we are hitting the swagger path, and not doing it locally as it just gets annoying :-)
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    // Get the encoded username and password
                    string encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                    // Decode from Base64 to string
                    string decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                    // Split username and password
                    string username = decodedUsernamePassword.Split(':', 2)[0];
                    string password = decodedUsernamePassword.Split(':', 2)[1];

                    // Check if login is correct
                    if (this.IsAuthorized(username, password))
                    {
                        await this.next.Invoke(context);
                        return;
                    }
                }

                // Return authentication type (causes browser to show login dialog)
                context.Response.Headers["WWW-Authenticate"] = "Basic";

                // Return unauthorized
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await this.next.Invoke(context);
            }
        }

        private bool IsAuthorized(string username, string password)
        {
            // Check that username and password are correct
            return username.Equals(this.swaggerAuthConfig.Username, StringComparison.InvariantCultureIgnoreCase)
                    && password.Equals(this.swaggerAuthConfig.Password);
        }
    }
}

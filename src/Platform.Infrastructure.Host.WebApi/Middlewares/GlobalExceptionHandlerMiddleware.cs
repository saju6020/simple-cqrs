// <copyright file="GlobalExceptionHandlerMiddleware.cs" company="Shohoz">
// Copyright © 2015-2020  All Rights Reserved.
// </copyright>

namespace Platform.Infrastructure.Host.WebApi.Middlewares
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Platform.Infrastructure.Host.WebApi.Constants;

    internal class GlobalExceptionHandlerMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;
        private readonly string errorVerbosity;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, IConfiguration configuration)
        {
            this.next = next;
            this.logger = logger;
            this.errorVerbosity = configuration["ErrorVerbosity"];
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception exception)
            {
                await this.HandleExceptionAsync(context, exception);
            }
        }

        private static string GetJwtToken(HttpContext context)
        {
            var oauthBearerTokenClaim = context.User.Claims.SingleOrDefault(c => c.Type.Equals("OauthBearerToken"));

            return oauthBearerTokenClaim == null ? string.Empty : oauthBearerTokenClaim.Value;
        }

        private static async Task<string> GetPayloadIfApplicable(HttpContext context)
        {
            var applicable = context.Request.Method.Equals(HttpMethods.Post, StringComparison.InvariantCultureIgnoreCase)
                 && context.Request.ContentType.Equals("application/json", StringComparison.InvariantCultureIgnoreCase);

            if (!applicable)
            {
                return "Empty";
            }

            using (var reader = new StreamReader(context.Request.Body))
            {
                var payload = await reader.ReadToEndAsync();
                return payload;
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var jwtToken = GetJwtToken(context);

            var requestPayload = await GetPayloadIfApplicable(context);

            var logMessage = $"Unhandled exception thrown on request [{context.Request.Method}] {context.Request.GetDisplayUrl()} : {exception.Message}.\r\nPayload: {requestPayload}\r\nToken: {jwtToken}";

            this.logger.LogError(exception, logMessage);

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            switch (this.errorVerbosity)
            {
                case ErrorVerbosities.None:
                    await JsonSerializer.SerializeAsync(context.Response.Body, "An error was encountered");
                    break;

                case ErrorVerbosities.ExceptionMessage:
                    await JsonSerializer.SerializeAsync(context.Response.Body, exception.Message);
                    break;

                case ErrorVerbosities.StackTrace:
                    await JsonSerializer.SerializeAsync(context.Response.Body, new SerializableExceptionWrapper(exception));
                    break;
            }
        }
    }
}

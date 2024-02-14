namespace Platform.Infrastructure.FaultTolerance.Http.Service
{
    using System;
    using System.Net.Http;
    using System.Collections.Generic;
    using Platform.Infrastructure.FaultTolerance.Http;
    using Platform.Infrastructure.FaultTolerance.Http.Contracts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// This class Exposes a method to get http client from httpclient factory.
    /// </summary>
    public class HttpClientProvider : IHttpClientProvider
    {
        /// <summary>
        /// private field of type IHttpClientFactory.
        /// </summary>
        private readonly IHttpClientFactory clientFactory;

        private readonly ILogger logger;

        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientProvider"/> class.
        /// </summary>
        /// <param name="clientFactory"> instance of IHttpClientFactory.</param>
        public HttpClientProvider(
            IHttpClientFactory clientFactory,
            ILogger<HttpClientProvider> logger,
            IHttpContextAccessor accessor)
        {
            this.clientFactory = clientFactory;
            this.logger = logger;
            this.httpContextAccessor = accessor;
        }

        /// <summary>
        /// Method responsible for providing HttpClient from HttpClientFactory.
        /// </summary>
        /// <returns> HttpClient.</returns>
        public HttpClient GetHttpClient()
        {
            this.logger.LogDebug("Inside GetHttpClient method");
            HttpClient httpClient = this.clientFactory.CreateClient(Constants.ClientName);
            this.PassCurrentContextHeader(httpClient);
            this.logger.LogDebug("Exiting GetHttpClient method");
            return httpClient;
        }

        public HttpClient GetHttpClient(HttpRequestMessage requestMessage)
        {
            HttpClient httpClient = this.clientFactory.CreateClient(Constants.ClientName);
            string bearerToken = this.httpContextAccessor.HttpContext.Request.Headers[Constants.Authorization];

            if (string.IsNullOrEmpty(bearerToken))
            {
                bearerToken = $"{Constants.Bearer} {this.GetTokenFromCookie()}";
            }

            requestMessage.Headers.Add(Constants.Authorization, bearerToken);

            return httpClient;
        }

        private string GetTokenFromCookie()
        {
            IRequestCookieCollection cookies = this.httpContextAccessor.HttpContext.Request.Cookies;

            string referer = this.httpContextAccessor.HttpContext.Request.Headers[Constants.Referer];

            foreach (KeyValuePair<string, string> cookie in cookies)
            {
                if (referer.Contains(cookie.Key))
                {
                    return cookie.Value;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Pass the current request header information to the new http client created from http client factory.
        /// </summary>
        /// <param name="httpClient"> HttpClient.</param>
        private void PassCurrentContextHeader(HttpClient httpClient)
        {
            this.logger.LogDebug("Entering PassCurrentHeaderContextToHttpClient method");
            /* Not implemented yet*/
            this.logger.LogDebug("Exiting PassCurrentHeaderContextToHttpClient method");
        }
    }
}

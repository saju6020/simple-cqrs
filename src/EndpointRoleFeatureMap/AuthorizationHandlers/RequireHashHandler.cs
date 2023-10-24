namespace Platform.Infrastructure.EndpointRoleFeatureMap.AuthorizationHandlers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;

    public class RequireHashHandler : AuthorizationHandler<RequireHash>
    {
        private const string HashHeaderName = "secret-key";
        private const string SecretKeyHash = "SecretKeyHash";
        private const string AlwaysValidateSecretKeyHash = "AlwaysValidateSecretKeyHash";

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<RequireHashHandler> logger;
        private readonly IConfiguration configuration;
        private readonly string hashToMatch;
        private readonly bool alwaysValidateSecretKeyHash;

        public RequireHashHandler(ILogger<RequireHashHandler> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            this.hashToMatch = this.configuration.GetValue<string>(SecretKeyHash) ?? throw new Exception($"No {SecretKeyHash} found in configuration.");
            this.alwaysValidateSecretKeyHash = this.configuration.GetValue<bool>(AlwaysValidateSecretKeyHash);
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequireHash requirement)
        {
            HttpRequest httpRequest = this.httpContextAccessor.HttpContext.Request;

            if (!httpRequest.Headers.ContainsKey(HashHeaderName) && !this.alwaysValidateSecretKeyHash)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            StringValues hashHeader = httpRequest.Headers[HashHeaderName];

            string hash = hashHeader.ToString();

            if (this.SafeEquals(hash, this.hashToMatch))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Checks if two strings are equal. Compares every char to prevent timing attacks.
        /// </summary>
        /// <param name="a">Lhs String to compare.</param>
        /// <param name="b">Rhs String to compare.</param>
        /// <returns>True if both strings are equal.</returns>
        private bool SafeEquals(string a, string b)
        {
            uint difference = (uint)a.Length ^ (uint)b.Length;

            for (int index = 0; index < a.Length && index < b.Length; index++)
            {
                difference |= (uint)a[index] ^ (uint)b[index];
            }

            return difference == 0;
        }
    }
}

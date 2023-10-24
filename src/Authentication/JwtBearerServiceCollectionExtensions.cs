namespace Platform.Infrastructure.Authentication
{
    using System;
    using System.Collections.Specialized;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Primitives;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Platform.Infrastructure.Common.Security;

    public static class JwtBearerServiceCollectionExtensions
    {
        private const string OriginHeaderName = "Origin";
        private const string RefererHeaderName = "Referer";
        private const string ServiceIdHeaderName = "x-service-id";
        private const string VerticalIdHeaderName = "vertical-id";
        private const string AuthorizationHeaderName = "Authorization";
        private const string XClientIdHeaderName = "X-ClientId";
        private const string AuthorizationServerAddressParameterName = "AuthorizationServerAddress";
        private const int TokenStartLength = 7;

        public static IServiceCollection AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            IdentityModelEventSource.ShowPII = true;

            services
           .AddAuthorization()
           .AddAuthentication(authenticationOptions =>
           {
               authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
           })
           .AddJwtBearer(options =>
           {
               options.Authority = configuration[AuthorizationServerAddressParameterName];

               options.RequireHttpsMetadata = false;

               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateAudience = false,
                   ValidateLifetime = true,
                   ClockSkew = TimeSpan.Zero,
               };

               var jwtSecurityTokenHandler = options.SecurityTokenValidators.First() as JwtSecurityTokenHandler;

               jwtSecurityTokenHandler.InboundClaimTypeMap.Clear();

               options.Events = new JwtBearerEvents
               {
                   OnTokenValidated = TokenValidated,
                   OnMessageReceived = MessageReceived,
               };
           });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            if (services.All(x => x.ServiceType != typeof(IUserContextProvider)))
            {
                services.AddSingleton<IUserContextProvider, HttpUserContextProvider>();
            }

            return services;
        }

        private static Task TokenValidated(TokenValidatedContext tokenValidatedContext)
        {
            var claimsIdentity = tokenValidatedContext.Principal.Identity as ClaimsIdentity;

            var serviceIdHeader = tokenValidatedContext.Request.Headers[ServiceIdHeaderName];
            var verticalIdHeader = tokenValidatedContext.Request.Headers[VerticalIdHeaderName];

            claimsIdentity.AddClaim(new Claim(Common.Security.ClaimTypes.ServiceId, serviceIdHeader.ToString().Trim()));

            var verticalHeaderStr = verticalIdHeader.ToString();
            var verticalClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == Common.Security.ClaimTypes.VerticalId);
            if (verticalClaim != null && !string.IsNullOrWhiteSpace(verticalHeaderStr))
            {
                claimsIdentity.RemoveClaim(verticalClaim);
            }

            claimsIdentity.AddClaim(new Claim(Common.Security.ClaimTypes.VerticalId, verticalIdHeader.ToString().Trim()));

            return Task.CompletedTask;
        }

        private static Task MessageReceived(MessageReceivedContext context)
        {
            context.Token = GetToken(context.Request);
            return Task.CompletedTask;
        }

        private static string GetToken(HttpRequest request)
        {
            var token = request.Headers[AuthorizationHeaderName];

            if (!token.Equals(StringValues.Empty))
            {
                if (token.ToString().Length > TokenStartLength)
                {
                    return token.ToString().Substring(TokenStartLength);
                }
                else
                {
                    return token.ToString();
                }
            }

            string tokenFromCooke = GetTokenFromCookie(request);

            if (!string.IsNullOrEmpty(tokenFromCooke))
            {
                return tokenFromCooke;
            }

            return GetTokenFromQueryString(request);
        }

        private static string GetTokenFromCookie(HttpRequest request)
        {
            if (request.Cookies == null || request.Cookies.Any() == false)
            {
                return string.Empty;
            }

            var originHost = GetHostOfRequestOrigin(request);

            return request.Cookies[originHost];
        }

        private static string GetHostOfRequestOrigin(HttpRequest request)
        {
            var origin = request.Headers[OriginHeaderName];

            if (origin.Equals(StringValues.Empty))
            {
                origin = request.Headers[RefererHeaderName];
            }

            if (origin.Equals(StringValues.Empty))
            {
                return string.Empty;
            }

            return new Uri(origin).Host;
        }

        private static string GetTokenFromQueryString(HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.QueryString.Value))
            {
                return string.Empty;
            }

            NameValueCollection parsedQuery = HttpUtility.ParseQueryString(request.QueryString.Value);
            if (parsedQuery["access_token"] != null)
            {
                return parsedQuery["access_token"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
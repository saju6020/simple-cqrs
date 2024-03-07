namespace Platform.Infrastructure.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using Microsoft.IdentityModel.Tokens;    
    using Platform.Infrastructure.Common.Security;
    using ClaimTypes = Platform.Infrastructure.Common.Security.ClaimTypes;

    /// <summary>
    /// Authentication helper class.
    /// </summary>
    public static class AuthenticationHelper
    {
        public static IServiceCollection AddJwtBearerAuthentication(
            IServiceCollection serviceCollection,
            TokenValidationOptions tokenValidationOptions,
            ILogger logger)
        {
            if (string.IsNullOrEmpty(tokenValidationOptions.JwtTokenVerificationPublicCertificatePath))
            {
                throw new Exception("Certificate path is empty");
            }

            List<string> tokenIssuers = new List<string> { tokenValidationOptions.TokenIssuer };

            List<CertificateInfo> certificateInfos = new List<CertificateInfo>
            {
                new CertificateInfo
                {
                    PublicCertificatePath = tokenValidationOptions.JwtTokenVerificationPublicCertificatePath,
                    Password = tokenValidationOptions.JwtTokenVerificationPublicCertificatePassword,
                },
            };

            IList<SecurityKey> securityKeys = BuildSigningKeys(certificateInfos, logger);

            Action<JwtBearerOptions> configureOptions =
                SetConfigurationOption(tokenValidationOptions, securityKeys, tokenIssuers, logger);

            // serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(configureOptions);
            serviceCollection.AddAuthentication(authenticationOptions =>
            {
                authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions);
            return serviceCollection;
        }

        private static Action<JwtBearerOptions> SetConfigurationOption(
            TokenValidationOptions tokenValidationOptions,
            IList<SecurityKey> securityKeys,
            IEnumerable<string> tokenIssuers,
            ILogger logger)
        {
            Action<JwtBearerOptions> configureOptions = options =>
            {
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = string.IsNullOrEmpty(tokenValidationOptions.Audience) ? "*" : tokenValidationOptions.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = tokenValidationOptions.ValidateAudience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuers = tokenIssuers,
                    IssuerSigningKeys = securityKeys,
                    ClockSkew = TimeSpan.Zero,
                    LifetimeValidator = GetLifeTimeValidator(logger),
                };

                var jwtSecurityTokenHandler = options.SecurityTokenValidators.First() as JwtSecurityTokenHandler;

                jwtSecurityTokenHandler.InboundClaimTypeMap.Clear();

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = MessageReceived,
                    OnTokenValidated = TokenValidated,
                };
            };

            return configureOptions;
        }

        private static LifetimeValidator GetLifeTimeValidator(ILogger logger)
        {
            LifetimeValidator lifetimeValidator = (
                DateTime? notBefore,
                DateTime? expires,
                SecurityToken securityToken,
                TokenValidationParameters validationParameters) =>
            {
                TokenValidationParameters clonedParameters = validationParameters.Clone();
                clonedParameters.LifetimeValidator = null;
                try
                {
                    Validators.ValidateLifetime(
                        notBefore,
                        expires,
                        securityToken,
                        clonedParameters);
                }
                catch (Exception ex)
                {
                    logger.LogInformation("Exception occured while validating token life time");
                    logger.LogInformation($"token valid till {securityToken.ValidTo}");
                    logger.LogError(ex.Message);

                    return false;
                }

                return true;
            };

            return lifetimeValidator;
        }

        private static Task TokenValidated(TokenValidatedContext context)
        {
            ClaimsIdentity claimsIdentity = context.Principal.Identity as ClaimsIdentity;

            var serviceIdHeader = context.Request.Headers["x-service-id"];

            if (!string.IsNullOrEmpty(serviceIdHeader))
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.ServiceId, serviceIdHeader));
            }
            else
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.ServiceId, string.Empty));
            }

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var currentUserContext = claimsPrincipal.GetAuthData();
            UserContext existingUserContext = context.HttpContext.RequestServices.GetRequiredService<UserContext>();
            existingUserContext.Set(currentUserContext);

            return Task.CompletedTask;
        }

        private static IList<SecurityKey> BuildSigningKeys(List<CertificateInfo> certificates, ILogger logger)
        {
            logger.LogInformation("Inside BuildSigningKeys method");
            IList<SecurityKey> securityKeyList = new List<SecurityKey>();

            try
            {
                securityKeyList = certificates
                    .Select(certificateInfo =>
                        new X509SecurityKey(new X509Certificate2(
                            certificateInfo.PublicCertificatePath,
                            certificateInfo.Password)))
                    .Cast<SecurityKey>().ToList();
            }
            catch (ArgumentNullException argumentNullEx)
            {
                logger.LogInformation("ArgumentNull exception occured while instantiating X509SecurityKey");
                logger.LogError(argumentNullEx.Message);
            }
            catch (CryptographicException ex)
            {
                logger.LogInformation("Cryptographic exception occured while loading public certificate");
                logger.LogError(ex.Message);

                throw new Exception(ex.Message);
            }

            logger.LogInformation("Exiting BuildSigningKeys method");

            return securityKeyList;
        }

        private static Task MessageReceived(MessageReceivedContext context)
        {
            context.Token = GetToken(context.Request);

            return Task.CompletedTask;
        }

        private static string GetToken(HttpRequest request)
        {
            var token = request.Headers["Authorization"];

            if (!token.Equals(StringValues.Empty))
            {
                return token.ToString().Substring(7);
            }

            string tokenFromCooke = GetTokenFromCookie(request);

            if (!string.IsNullOrEmpty(tokenFromCooke))
            {
                return tokenFromCooke;
            }

            return GetTokenFromQueryString(request);
        }

        private static string GetTokenFromQueryString(HttpRequest request)
        {
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

        private static string GetTokenFromCookie(HttpRequest request)
        {
            if (request.Cookies.Any() == false)
            {
                return string.Empty;
            }

            var originHost = GetHostOfRequestOrigin(request);

            return request.Cookies[originHost];
        }

        private static string GetHostOfRequestOrigin(HttpRequest request)
        {
            var origin = request.Headers["Origin"];

            if (origin.Equals(StringValues.Empty))
            {
                origin = request.Headers["Referer"];
            }

            if (origin.Equals(StringValues.Empty))
            {
                return string.Empty;
            }

            return new Uri(origin).Host;
        }
    }
}

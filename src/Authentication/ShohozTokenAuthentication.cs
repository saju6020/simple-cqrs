namespace Platform.Infrastructure.Authentication
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Provides extension method of IServiceCollection.
    /// </summary>
    public static class ShohozTokenAuthentication
    {
        /// <summary>
        /// extension method of IServiceCollection.
        /// </summary>
        /// <param name="serviceCollection">IServiceCollection.</param>
        /// <param name="configuration">IConfiguration.</param>
        /// <returns>returns IServiceCollection.</returns>
        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration, ILogger logger)
        {
            TokenValidationOptions tokenValidationConfig = new TokenValidationOptions();

            configuration.GetSection(Constants.TokenValidationOptionsKey).Bind(tokenValidationConfig);

            return AuthenticationHelper.AddJwtBearerAuthentication(serviceCollection, tokenValidationConfig, logger);
        }
    }
}
